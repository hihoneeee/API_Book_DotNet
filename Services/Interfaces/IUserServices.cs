using TestWebAPI.DTOs.User;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IUserServices
    {
        Task<ServiceResponse<GetUserDTO>> GetCurrentAsync(int id);
        Task<ServiceResponse<UserDTO>> UpdateProfileUserAsync(int id, UserDTO userDTO);
        Task<ServiceResponse<AvatarUserDTO>> UpdateAvatarUserAsync(int id, AvatarUserDTO avatarUserDTO);
    }
}
