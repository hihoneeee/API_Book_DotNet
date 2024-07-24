using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
[Authorize]
public class ChatHub : Hub
{
    private readonly IChatHubRepositories _chatHubRepo;
    private readonly IUserRepositories _userRepo;
    private readonly IMapper _mapper;
    private readonly IPropertyRepositories _propertyRepo;
    private readonly IPropertyHasDetailRepositories _propertyHasDetailRepo;
    private readonly INotificationRepositories _notificationRepo;

    public ChatHub(IMapper mapper,IChatHubRepositories chatHubRepo, IUserRepositories userRepo, IPropertyRepositories propertyRepo, IPropertyHasDetailRepositories propertyHasDetailRepo, INotificationRepositories notificationRepo)
    {
        _chatHubRepo = chatHubRepo;
        _userRepo = userRepo;
        _mapper = mapper;
        _propertyRepo = propertyRepo;
        _propertyHasDetailRepo = propertyHasDetailRepo;
        _notificationRepo = notificationRepo;
    }
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
    public async Task SendNotification(int userId, string content)
    {
        await Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", content);
    }

    public async Task SendMessageAysnc(MessageDTO messageDTO)
    {
        try
        {
            var message = _mapper.Map<Message>(messageDTO);
            var sendMessage = await _chatHubRepo.SendMessage(message);
            var conversation = await _chatHubRepo.GetConversationById(messageDTO.conversationId);
            var user1 = await _userRepo.GetCurrentAsync(conversation.userId1);
            var user2 = await _userRepo.GetCurrentAsync(conversation.userId2);
            if (conversation.userId1 != messageDTO.userId)
            {
                var content = $"{user2.first_name}{user2.last_name} has sent you a new message!";
                await Clients.Group(conversation.userId1.ToString()).SendAsync("ReceiveNotification", content);
            }

            if (conversation.userId2 != messageDTO.userId)
            {
                var content = $"{user1.first_name}{user1.last_name} has sent you a new message!";
                await Clients.Group(conversation.userId2.ToString()).SendAsync("ReceiveNotification", content);
            }

            await Clients.Group(messageDTO.conversationId.ToString()).SendAsync("ReceiveMessage", messageDTO.userId, messageDTO.content, messageDTO.createdAt);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in SendMessageAsync: {ex.Message}");
            throw new HubException("An error occurred while sending the message.", ex);
        }
    }

    public async Task<ServiceResponse<NotificationDTO>> CreateAppointmentNotificationAsync(int propertyId, int buyerId)
    {
        var serviceResponse = new ServiceResponse<NotificationDTO>();
        try
        {
            var checkProperty = await _propertyRepo.GetPropertyByIdAsync(propertyId);
            if (checkProperty != null)
            {
                var checkPropertyHasDetail = await _propertyHasDetailRepo.GetDetailByIdAsync(propertyId);
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

                        await Clients.Group(checkUser.id.ToString()).SendAsync("ReceiveNotification", notification.content);
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
    public async Task OnConnectedAsync(int id, string connectionId)
    {
        await Groups.AddToGroupAsync(connectionId, id.ToString());
    }

    public async Task OnDisconnectedAsync(int id, string connectionId)
    {
        await Groups.RemoveFromGroupAsync(connectionId, id.ToString());
    }
}
