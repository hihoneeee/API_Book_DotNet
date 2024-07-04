using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class RealTimeServices : IRealTimeServices
    {
        private readonly IChatHubRepositories _chatHubRepo;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotificationRepositories _notificationRepo;
        private readonly IPropertyRepositories _propertyRepo;
        private readonly IPropertyHasDetailRepositories _propertyHasDetailRepo;
        private readonly IUserRepositories _userRepo;
        private readonly IMapper _mapper;

        public RealTimeServices(IChatHubRepositories chatHubRepo, IMapper mapper, IHubContext<ChatHub> hubContext, INotificationRepositories notificationRepo, IPropertyRepositories propertyRepo, IPropertyHasDetailRepositories propertyHasDetailRepo, IUserRepositories userRepo) {
            _chatHubRepo = chatHubRepo;
            _mapper = mapper;
            _hubContext = hubContext;
            _notificationRepo = notificationRepo;
            _propertyRepo = propertyRepo;
            _propertyHasDetailRepo = propertyHasDetailRepo;
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<GetConversationDTO>> GetOrCreateConversation(ConversationDTO conversationDTO)
        {
            var serviceResponse = new ServiceResponse<GetConversationDTO>();
            try
            {
                var checkConversation = await _chatHubRepo.CheckkConversation(conversationDTO.userId1, conversationDTO.userId2);
                if (checkConversation != null)
                {
                    var messages = await _chatHubRepo.GetMessagesForConversation(checkConversation.id);
                    var conversationWithMessages = new GetConversationDTO
                    {
                        id = checkConversation.id,
                        userId1 = checkConversation.userId1,
                        userId2 = checkConversation.userId2,
                        createdAt = checkConversation.createdAt,
                        dataMessages = messages.Select(m => new GetMessageDTO
                        {
                            conversationId = m.conversationId,
                            userId = m.userId,
                            content = m.content,
                            createdAt = DateTime.UtcNow,
                        }).ToList()
                    };
                    serviceResponse.data = conversationWithMessages;
                    serviceResponse.SetSuccess("Conversation exists, returning messages.");
                }
                else
                {
                    var conversation = _mapper.Map<Conversation>(conversationDTO);
                    var createConversation = await _chatHubRepo.CreateConversation(conversation);
                    serviceResponse.data = _mapper.Map<GetConversationDTO>(createConversation);
                    serviceResponse.SetSuccess("Create successfully!");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<MessageDTO>> SendMessage(MessageDTO messageDTO)
        {
            var serviceResponse = new ServiceResponse<MessageDTO>();
            try
            {
                var message = _mapper.Map<Message>(messageDTO);
                var sendMessage = await _chatHubRepo.SendMessage(message);
                serviceResponse.SetSuccess("Message sent successfully!");

                var conversation = await _chatHubRepo.GetConversationById(messageDTO.conversationId);
                var user1 = await _userRepo.GetCurrentAsync(conversation.userId1);
                var user2 = await _userRepo.GetCurrentAsync(conversation.userId2);
                if (conversation.userId1 != messageDTO.userId)
                {
                    var content = $"{user2.first_name}{user2.last_name} has sent you a new message!";
                    await _hubContext.Clients.Group(conversation.userId1.ToString()).SendAsync("ReceiveNotification", content);
                }

                if (conversation.userId2 != messageDTO.userId)
                {
                    var content = $"{user1.first_name}{user1.last_name} has sent you a new message!";
                    await _hubContext.Clients.Group(conversation.userId2.ToString()).SendAsync("ReceiveNotification", content);
                }

                await _hubContext.Clients.Group(messageDTO.conversationId.ToString()).SendAsync("ReceiveMessage", messageDTO.userId, messageDTO.content, messageDTO.createdAt);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task OnConnectedAsync(int id, string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, id.ToString());
        }

        public async Task OnDisconnectedAsync(int id, string connectionId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, id.ToString());
        }

        public async Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId)
        {
            var serviceResponse = new ServiceResponse<List<GetMessageDTO>>();
            try
            {
                var messages = await _chatHubRepo.GetMessagesForConversation(conversationId);
                serviceResponse.data = _mapper.Map<List<GetMessageDTO>>(messages);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<NotificationDTO>> CreateAppointmentNotificationAsync(int propertyId, int buyerId)
        {
            var serviceResponse = new ServiceResponse<NotificationDTO>();
            try
            {
                var checkProperty = await _propertyRepo.GetPropertyByIdAsync(propertyId);
                if (checkProperty != null)
                {
                    var checkPropertyHasDetail = await _propertyHasDetailRepo.GetPropertyHasDetailByPropertyIdAsync(propertyId);
                    if (checkPropertyHasDetail != null)
                    {
                        var checkUser = await _userRepo.GetCurrentAsync(checkPropertyHasDetail.sellerId);
                        if (checkUser != null)
                        {
                            var notification = new Notification
                            {
                                userId = checkUser.id,
                                content = $"Có một cuộc hẹn mới được đặt cho bất động sản: {checkProperty.title}",
                                createdAt = DateTime.Now,
                                buyerId = buyerId,
                                IsRead = false
                            };
                            var createdNotification = await _notificationRepo.CreateNoficationsAsync(notification);
                            serviceResponse.data = _mapper.Map<NotificationDTO>(createdNotification);
                            serviceResponse.SetSuccess("Appointment notification created successfully!");

                            await _hubContext.Clients.Group(checkUser.id.ToString()).SendAsync("ReceiveNotification", notification.content);
                        }
                        else
                        {
                            serviceResponse.SetError("User not found.");
                        }
                    }
                    else
                    {
                        serviceResponse.SetError("Property detail not found.");
                    }
                }
                else
                {
                    serviceResponse.SetError("Property not found.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetNotificationDTO>>> GetNotificationsForUser(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetNotificationDTO>>();
            try
            {
                var notifications = await _notificationRepo.GetNotificationsForUser(userId);
                serviceResponse.data = _mapper.Map<List<GetNotificationDTO>>(notifications);
                serviceResponse.SetSuccess("Notifications fetched successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> MarkNotificationsAsRead(int userId)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                await _notificationRepo.MarkNotificationsAsRead(userId);
                serviceResponse.data = true;
                serviceResponse.SetSuccess("Notifications marked as read successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                serviceResponse.data = false;
            }
            return serviceResponse;
        }
    }
}
