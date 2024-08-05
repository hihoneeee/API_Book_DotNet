using TestWebAPI.DTOs.Auth;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthRegisterDTO>> Register(AuthRegisterDTO authLoginDTO);
        Task<ServiceResponse<AuthLoginDTO>> Login(AuthLoginDTO authLoginDTO);
        Task<ServiceResponse<AuthLoginDTO>> LoginMvc(AuthLoginDTO authLoginDTO);
        Task<ServiceResponse<RefreshTokenDTO>> refreshTokenAsync(string refreshToken);
        Task<ServiceResponse<AuthForgotPasswordDTO>> ForgotPassword(string email);
        Task<ServiceResponse<AuthResetPasswordDTO>> ResetPasswordAsync(string password, string token);
        Task<ServiceResponse<AuthChangePasswordDTO>> ChangePasswordasync(AuthChangePasswordDTO authChangePasswordDTO);
    }
}
