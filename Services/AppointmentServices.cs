using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TestWebAPI.DTOs.Appointment;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentRepositories _appointmentRepo;
        private readonly IRealTimeServices _realTimeServices;
        private readonly IPropertyRepositories _propertyRepo;
        public IHubContext<ChatHub> _chatHub { get; }

        public AppointmentServices(IMapper mapper, IAppointmentRepositories appointmentRepo, IRealTimeServices realTimeServices, IHubContext<ChatHub>  chatHub, IPropertyRepositories propertyRepo) {
            _mapper = mapper;
            _appointmentRepo = appointmentRepo;
            _realTimeServices = realTimeServices;
            _chatHub = chatHub;
            _propertyRepo = propertyRepo;
        }
        public async Task<ServiceResponse<GetAppointmentDTO>> CreateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            var serviceResponse = new ServiceResponse<GetAppointmentDTO>();
            try
            {
                var property = await _propertyRepo.GetPropertyByIdAsync(appointmentDTO.propertyId);
                if (property.PropertyHasDetail.sellerId == appointmentDTO.buyerId)
                {
                    serviceResponse.SetBadRequest("You cannot set your own schedule!");
                    return serviceResponse;
                }
                var appointment = _mapper.Map<Appointment>(appointmentDTO);
                var createdAppointment = await _appointmentRepo.CreateAppointmentAsync(appointment);
                serviceResponse.data = _mapper.Map<GetAppointmentDTO>(createdAppointment);
                serviceResponse.SetSuccess("Send appointment successfully!");

                var notificationResponse = await _realTimeServices.CreateAppointmentNotificationAsync(createdAppointment.propertyId, createdAppointment.buyerId);
                if (!notificationResponse.success)
                {
                    serviceResponse.SetError(notificationResponse.message);
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
