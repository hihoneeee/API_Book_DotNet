using TestWebAPI.DTOs.Auth;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthRegisterDTO>> Register(AuthRegisterDTO authLoginDTO);
        Task<ServiceResponse<AuthLoginDTO>> Login(AuthLoginDTO authLoginDTO);
        Task<ServiceResponse<refreshToken>> refreshTokenAsync(string refreshToken);
        Task<ServiceResponse<AuthChangePasswordDTO>> ForgotPassword(string email);
        Task<ServiceResponse<AuthChangePasswordDTO>> ResetPasswordAsync(string password, string token);
    }
}
