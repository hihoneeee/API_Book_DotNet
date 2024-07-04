using TestWebAPI.DTOs.Appointment;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IAppointmentServices
    {
        Task<ServiceResponse<GetAppointmentDTO>> CreateAppointmentAsync(AppointmentDTO appointmentDTO);
    }
}
