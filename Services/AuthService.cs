using AutoMapper;
using Microsoft.AspNetCore.Http;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.DTOs.User;
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

        public AuthService(IMapper mapper, IAuthRepositories authRepo, IJWTHelper jWTHelper, IJwtService jwtService, IHttpContextAccessor httpContextAccessor) { 
            _mapper = mapper;
            _jwtService = jwtService;
            _authRepo = authRepo ?? throw new ArgumentNullException(nameof(authRepo));
            _jWTHelper = jWTHelper ?? throw new ArgumentNullException(nameof(jWTHelper));
            _httpContextAccessor = httpContextAccessor;

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
                string token = await _jWTHelper.GenerateJWTToken(existingUser.id, existingUser.roleCode, DateTime.UtcNow.AddMinutes(10));
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

        public async Task<ServiceResponse<refreshToken>> refreshTokenAsync(string refreshToken)
        {
            var serviceResponse = new ServiceResponse<refreshToken>();
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
    }
}
