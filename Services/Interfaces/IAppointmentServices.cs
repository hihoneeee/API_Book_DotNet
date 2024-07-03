using TestWebAPI.DTOs.Appointment;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IAppointmentServices
    {
        Task<ServiceResponse<AppointmentDTO>> CreateAppointmentAsync(AppointmentDTO appointmentDTO);
    }
}
