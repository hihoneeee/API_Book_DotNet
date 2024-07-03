using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IAppointmentRepositories
    {
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    }
}
