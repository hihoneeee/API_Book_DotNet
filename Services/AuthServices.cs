using AutoMapper;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.Helpers;
using TestWebAPI.Middlewares;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IJwtServices _jwtService;
        private readonly IAuthRepositories _authRepo;
        private readonly IJWTHelper _jWTHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepositories _userRepo;
        private readonly IRoleRepositories _roleRepo;

        public AuthServices(IMapper mapper, IAuthRepositories authRepo, IJWTHelper jWTHelper, IJwtServices jwtService, IHttpContextAccessor httpContextAccessor, IUserRepositories userRepo, IRoleRepositories roleRepo)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _authRepo = authRepo ?? throw new ArgumentNullException(nameof(authRepo));
            _jWTHelper = jWTHelper ?? throw new ArgumentNullException(nameof(jWTHelper));
            _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }
            
        public async Task<ServiceResponse<AuthRegisterDTO>> Register(AuthRegisterDTO authRegisterDTO)
        {
            var serviceResponse = new ServiceResponse<AuthRegisterDTO>();
            try
            {
                var existingEmail = await _authRepo.getByEmail(authRegisterDTO.email);
                if (existingEmail != null)
                {
                    serviceResponse.SetExisting("Email already exists!");
                    return serviceResponse;
                }
                var checkRole = await _roleRepo.GetRoleByCodeAsyn(authRegisterDTO.roleCode);
                if (checkRole == null)
                {
                    serviceResponse.SetNotFound("Role");
                    return serviceResponse;
                }
                var newUser = _mapper.Map<User>(authRegisterDTO);
                newUser.password = HashPasswordHelper.HashPassword(authRegisterDTO.password);
                var Response = await _authRepo.Register(newUser);
                serviceResponse.SetSuccess("Register successfully");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetNotFound("Email");
                    return serviceResponse;
                }
                if (!HashPasswordHelper.VerifyPassword(authLoginDTO.password, existingUser.password))
                {
                    serviceResponse.SetUnauthorized("Password is wrong!");
                    return serviceResponse;
                }
                // Tạo JWT token và refresh token
                string token = await _jWTHelper.GenerateJWTToken(existingUser.id, existingUser.roleCode, DateTime.UtcNow.AddMinutes(5));
                string refresh_token = await _jWTHelper.GenerateJWTRefreshToken(existingUser.id, existingUser.roleCode, DateTime.UtcNow.AddMonths(30));

                // Thêm refresh token vào trong bảng JWT
                await _jwtService.InsertJWTToken(new jwtDTO()
                {
                    userId = existingUser.id,
                    expiredDate = DateTime.UtcNow.AddDays(30),
                    value = refresh_token,
                    issuedDate = DateTime.UtcNow
                });

                // Lưu refresh token vào cookie
                string cookieName = "refresh_token";
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(30)
                };
                _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, refresh_token, cookieOptions);
                serviceResponse.accessToken = token;
                serviceResponse.SetSuccess("Login successfully");
               
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetUnauthorized("Invalid refresh token!");
                    return serviceResponse;
                }

                // Lấy thông tin từ refresh token
                var userId = _jWTHelper.GetUserIdFromToken(refreshToken);
                var userRole = _jWTHelper.GetUserRoleFromToken(refreshToken);

                // Tạo mới access token
                string accessToken = await _jWTHelper.GenerateJWTToken(userId, userRole, DateTime.UtcNow.AddMinutes(10));

                serviceResponse.accessToken = accessToken;
                serviceResponse.SetSuccess("Access token refreshed successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetNotFound("Email");
                    return serviceResponse;
                }

                var authChangePassword = await _authRepo.InsertChangePasswordAsyn(existsEmail);
                serviceResponse.data = _mapper.Map<AuthForgotPasswordDTO>(authChangePassword);
                serviceResponse.SetSuccess("Password reset token generated!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetNotFound("Token");
                    return serviceResponse;
                }
                var restPassword = await _authRepo.ResetNewPasswordAsync(newPassword, findPasswordToken);
                serviceResponse.SetSuccess("Password change succssefully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                if (!HashPasswordHelper.VerifyPassword(authChangePasswordDTO.oldPassword, checkUSer.password))
                {
                    serviceResponse.SetUnauthorized("Password is wrong!");
                    return serviceResponse;
                }
                if (authChangePasswordDTO.newPassword != authChangePasswordDTO.enterPassword)
                {
                    serviceResponse.SetBadRequest("New password and confirmation password do not match!");
                    return serviceResponse;
                }
                var HashPassword = HashPasswordHelper.HashPassword(authChangePasswordDTO.newPassword);
                var changePassword = await _authRepo.ChangePasswordAsync(HashPassword, checkUSer);
                serviceResponse.SetSuccess("Password change succssefully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
        
    }
}
