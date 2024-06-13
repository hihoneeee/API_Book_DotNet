using TestWebAPI.DTOs.User;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IUserServices
    {
        Task<ServiceResponse<UserDTO>> GetCurrentAsync(int id);
    }
}
