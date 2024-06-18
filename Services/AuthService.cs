using AutoMapper;
using NuGet.Common;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.Helpers;
using TestWebAPI.Middlewares;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IAuthRepositories _authRepo;
        private readonly IJWTHelper _jWTHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepositories _userRepo;

        public AuthService(IMapper mapper, IAuthRepositories authRepo, IJWTHelper jWTHelper, IJwtService jwtService, IHttpContextAccessor httpContextAccessor, IUserRepositories userRepo)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _authRepo = authRepo ?? throw new ArgumentNullException(nameof(authRepo));
            _jWTHelper = jWTHelper ?? throw new ArgumentNullException(nameof(jWTHelper));
            _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
        }
            
        public async Task<ServiceResponse<AuthRegisterDTO>> Register(AuthRegisterDTO authRegisterDTO)
        {
            var serviceResponse = new ServiceResponse<AuthRegisterDTO>();
            try
            {
                var existingEmail = await _authRepo.getByEmail(authRegisterDTO.email);
                if (existingEmail != null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Email already exists.";
                    serviceResponse.statusCode = EHttpType.Conflict;
                    return serviceResponse;
                }
                var newUser = _mapper.Map<User>(authRegisterDTO);
                newUser.password = HashPasswordHelper.HashPassword(authRegisterDTO.password);
                var Response = await _authRepo.Register(newUser);
                serviceResponse.message = "Register successfully.";
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<AuthLoginDTO>> Login(AuthLoginDTO authLoginDTO)
        {
            var serviceResponse = new ServiceResponse<AuthLoginDTO>();
            try
            {
                var existingUser = await _authRepo.getByEmail(authLoginDTO.email);
                if (existingUser == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Email does not exist.";
                    serviceResponse.statusCode = EHttpType.Unauthorized;
                    return serviceResponse;
                }
                if (!HashPasswordHelper.VerifyPassword(authLoginDTO.password, existingUser.password))
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Password is wrong!.";
                    serviceResponse.statusCode = EHttpType.Unauthorized;
                    return serviceResponse;
                }
                // Tạo JWT token và refresh token
                string token = await _jWTHelper.GenerateJWTToken(existingUser.id, existingUser.roleCode, DateTime.UtcNow.AddMinutes(5));
                string refresh_token = await _jWTHelper.GenerateJWTRefreshToken(existingUser.id, DateTime.UtcNow.AddMonths(30));

                // Thêm refresh token vào trong bảng JWT
                await _jwtService.InsertJWTToken(new jwtDTO()
                {
                    user_id = existingUser.id,
                    expired_date = DateTime.UtcNow.AddDays(30),
                    value = refresh_token,
                    issued_date = DateTime.UtcNow
                });

                // Lưu refresh token vào cookie
                string cookieName = "refresh_token";
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(30) // Thời gian sống của cookie là 7 ngày
                };
                _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, refresh_token, cookieOptions);

                serviceResponse.success = true;
                serviceResponse.message = "Login successful.";
                serviceResponse.access_token = token;
                serviceResponse.statusCode = EHttpType.Success;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RefreshTokenDTO>> refreshTokenAsync(string refreshToken)
        {
            var serviceResponse = new ServiceResponse<RefreshTokenDTO>();
            try
            {
                // Kiểm tra tính hợp lệ của refresh token
                bool isTokenValid = await _jWTHelper.ValidateRefreshTokenAsync(refreshToken);
                if (!isTokenValid)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Invalid refresh token.";
                    serviceResponse.statusCode = EHttpType.Unauthorized;
                    return serviceResponse;
                }

                // Lấy thông tin từ refresh token
                var userId = _jWTHelper.GetUserIdFromToken(refreshToken);
                var userRole = _jWTHelper.GetUserRoleFromToken(refreshToken);

                // Tạo mới access token
                string accessToken = await _jWTHelper.GenerateJWTToken(userId, userRole, DateTime.UtcNow.AddMinutes(10));

                serviceResponse.success = true;
                serviceResponse.message = "Access token refreshed successfully.";
                serviceResponse.access_token = accessToken;
                serviceResponse.statusCode = EHttpType.Success;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An error occurred while refreshing access token: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;

            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<AuthForgotPasswordDTO>> ForgotPassword(string email)
        {
            var serviceResponse = new ServiceResponse<AuthForgotPasswordDTO>();
            try
            {
                var existsEmail = await _authRepo.getByEmail(email);
                if (existsEmail == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Email not found.";
                    serviceResponse.statusCode = EHttpType.NotFound;
                    return serviceResponse;
                }

                var authChangePassword = await _authRepo.InsertChangePasswordAsyn(existsEmail);

                // Here we add the token to the service response
                serviceResponse.data = _mapper.Map<AuthForgotPasswordDTO>(authChangePassword);
                serviceResponse.success = true;
                serviceResponse.message = "Password reset token generated.";
                serviceResponse.statusCode = EHttpType.Success;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An error occurred while processing your request: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AuthResetPasswordDTO>> ResetPasswordAsync(string password, string token)
        {
            var serviceResponse = new ServiceResponse<AuthResetPasswordDTO>();
            try
            {
                var newPassword = HashPasswordHelper.HashPassword(password);
                var findPasswordToken = await _authRepo.FindPasswordResetTokenAsyn(token);
                if (findPasswordToken == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Token don't have existing!";
                    serviceResponse.statusCode = EHttpType.NotFound;
                    return serviceResponse;
                }
                var restPassword = await _authRepo.ResetNewPasswordAsync(newPassword, findPasswordToken);
                serviceResponse.success = true;
                serviceResponse.message = "Password change succssefully!";
                serviceResponse.statusCode = EHttpType.Success;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An error occurred while processing your request: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }

            return serviceResponse;
        }
        
        public async Task<ServiceResponse<AuthChangePasswordDTO>> ChangePasswordasync(AuthChangePasswordDTO authChangePasswordDTO)
        {
            var serviceResponse = new ServiceResponse<AuthChangePasswordDTO>();
            try
            {
                var checkUSer = await _userRepo.GetCurrentAsync(authChangePasswordDTO.id);
                if (checkUSer == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "User not found!";
                    serviceResponse.statusCode = EHttpType.NotFound;
                    return serviceResponse;
                }
                if (!HashPasswordHelper.VerifyPassword(authChangePasswordDTO.oldPassword, checkUSer.password))
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Password is wrong!.";
                    serviceResponse.statusCode = EHttpType.Unauthorized;
                    return serviceResponse;
                }
                if (authChangePasswordDTO.newPassword != authChangePasswordDTO.enterPassword)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "New password and confirmation password do not match!";
                    serviceResponse.statusCode = EHttpType.BadRequest;
                    return serviceResponse;
                }
                var HashPassword = HashPasswordHelper.HashPassword(authChangePasswordDTO.newPassword);
                var changePassword = await _authRepo.ChangePasswordAsync(HashPassword, checkUSer);
                serviceResponse.success = true;
                serviceResponse.message = "Password change succssefully!";
                serviceResponse.statusCode = EHttpType.Success;

            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An error occurred while processing your request: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }

            return serviceResponse;
        }
        
    }
}
