using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.DTOs.User;
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

        public async Task<ServiceResponse<GetNotificationDTO>> CreateAppointmentNotificationAsync(int propertyId, int buyerId)
        {
            var serviceResponse = new ServiceResponse<GetNotificationDTO>();
            try
            {
                var checkProperty = await _propertyRepo.GetPropertyByIdAsync(propertyId);
                if (checkProperty != null)
                {
                    var checkPropertyHasDetail = await _propertyHasDetailRepo.GetDetailByIdAsync(propertyId);
                    if (checkPropertyHasDetail != null)
                    {
                        var checkBuyerId = await _userRepo.GetCurrentAsync(buyerId);
                        var checkUser = await _userRepo.GetCurrentAsync(checkPropertyHasDetail.sellerId);
                        if (checkUser != null)
                        {
                            var notificationDTO = new GetNotificationDTO
                            {
                                userId = checkUser.id,
                                content = $"Có một cuộc hẹn mới được đặt cho bất động sản: {checkProperty.title}",
                                createdAt = DateTime.Now,
                                buyerId = buyerId,
                                IsRead = false,
                                dataUser = new GetUserDTO
                                {
                                    avatar = checkBuyerId.avatar,
                                    first_name = checkBuyerId.first_name,
                                    last_name = checkBuyerId.last_name,
                                    id = checkBuyerId.id,
                                }
                            };
                            var notification = _mapper.Map<Notification>(notificationDTO);
                            var createdNotification = await _notificationRepo.CreateNoficationsAsync(notification);
                            serviceResponse.data = _mapper.Map<GetNotificationDTO>(createdNotification);
                            serviceResponse.SetSuccess("Appointment notification created successfully!");

                            await _hubContext.Clients.Group(checkUser.id.ToString()).SendAsync("ReceiveNotification", serviceResponse.data);
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
    }
}
