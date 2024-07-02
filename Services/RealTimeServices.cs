using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TestWebAPI.DTOs.Category;
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
        private readonly IMapper _mapper;

        public RealTimeServices(IChatHubRepositories chatHubRepo, IMapper mapper, IHubContext<ChatHub> hubContext, INotificationRepositories notificationRepo) {
            _chatHubRepo = chatHubRepo;
            _mapper = mapper;
            _hubContext = hubContext;
            _notificationRepo = notificationRepo;
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

                if (conversation.userId1 != messageDTO.userId)
                {
                    Console.WriteLine("Sending notification to user: " + conversation.userId1);
                    await _hubContext.Clients.User(conversation.userId1.ToString()).SendAsync("ReceiveNotification", conversation.userId1, "New message in conversation " + messageDTO.conversationId);
                }

                if (conversation.userId2 != messageDTO.userId)
                {
                    Console.WriteLine("Sending notification to user: " + conversation.userId2);
                    await _hubContext.Clients.User(conversation.userId2.ToString()).SendAsync("ReceiveNotification", conversation.userId2, "New message in conversation " + messageDTO.conversationId);
                }

                await _hubContext.Clients.Group(messageDTO.conversationId.ToString()).SendAsync("ReceiveMessage", messageDTO.userId, messageDTO.content, messageDTO.createdAt);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task JoinConversation(int conversationId, string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, conversationId.ToString());
        }

        public async Task LeaveConversation(int conversationId, string connectionId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, conversationId.ToString());
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

        public async Task<ServiceResponse<NotificationDTO>> CreateNotification(NotificationDTO notificationDTO)
        {
            var serviceResponse = new ServiceResponse<NotificationDTO>();
            try
            {
                var notification = _mapper.Map<Notification>(notificationDTO);
                notification.content = $"New message from user {notification.userId}";
                var createdNotification = await _notificationRepo.CreateNoficationsAsync(notification);
                serviceResponse.SetSuccess("Notification created successfully!");
                await _hubContext.Clients.Group(notificationDTO.conversationId.ToString()).SendAsync("ReceiveNotification", notificationDTO.userId, notificationDTO.content, notificationDTO.createdAt);
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
