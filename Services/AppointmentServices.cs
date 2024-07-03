using AutoMapper;
using TestWebAPI.DTOs.Appointment;
using TestWebAPI.DTOs.Notification;
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

        public AppointmentServices(IMapper mapper, IAppointmentRepositories appointmentRepo, IRealTimeServices realTimeServices) {
            _mapper = mapper;
            _appointmentRepo = appointmentRepo;
            _realTimeServices = realTimeServices;
        }
        public async Task<ServiceResponse<AppointmentDTO>> CreateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            var serviceResponse = new ServiceResponse<AppointmentDTO>();
            try
            {
                var appointment = _mapper.Map<Appointment>(appointmentDTO);
                var createdAppointment = await _appointmentRepo.CreateAppointmentAsync(appointment);
                serviceResponse.SetSuccess("Appointment created successfully!");

                var notificationResponse = await _realTimeServices.CreateAppointmentNotificationAsync(createdAppointment.propertyId);
                if (!notificationResponse.success)
                {
                    serviceResponse.SetError(notificationResponse.message);
                }

                serviceResponse.data = _mapper.Map<AppointmentDTO>(createdAppointment);
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
