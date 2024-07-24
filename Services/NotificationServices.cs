using AutoMapper;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Helpers;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class NotificationServices : INotificationServices
    {
        public readonly IMapper _mapper;
        public readonly INotificationRepositories _notificationRepo;

        public NotificationServices(IMapper mapper, INotificationRepositories notificationRepo) {
            _mapper = mapper;
            _notificationRepo = notificationRepo;
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
