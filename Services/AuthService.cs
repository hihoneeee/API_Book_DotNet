using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using TestWebAPI.DTOs.Auth;
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
        private readonly IAuthRepositories _authRepo;
        private readonly IJWTHelper _jWTHelper;

        public AuthService(IMapper mapper, IAuthRepositories authRepo, IJWTHelper jWTHelper) { 
            _mapper = mapper;
            _authRepo = authRepo ?? throw new ArgumentNullException(nameof(authRepo));
            _jWTHelper = jWTHelper ?? throw new ArgumentNullException(nameof(jWTHelper));
        }

        public async Task<ServiceResponse<AuthRegisterDTO>> Register(AuthRegisterDTO authRegisterDTO)
        {
            var serviceResponse = new ServiceResponse<AuthRegisterDTO>();
            try
            {
                var existingEmail = await _authRepo.getByEmail(authRegisterDTO.email);
                if (existingEmail != null)
                {
                    serviceResponse.statusCode = EHttpType.BadRequest;
                    serviceResponse.success = false;
                    serviceResponse.message = "Email already exists.";
                }
                var newUser = _mapper.Map<User>(authRegisterDTO);
                newUser.password = HashPasswordHelper.HashPassword(authRegisterDTO.password);
                var Response = await _authRepo.Register(newUser);
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.message = "Register successfully.";
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

        public async Task<ServiceResponse<CredentialDTO>> Login(AuthLoginDTO authLoginDTO)
        {
            var serviceResponse = new ServiceResponse<CredentialDTO>();
            try
            {
                var existingUser = await _authRepo.getByEmail(authLoginDTO.email);
                if (existingUser == null)
                {
                    serviceResponse.statusCode = EHttpType.BadRequest;
                    serviceResponse.success = false;
                    serviceResponse.message = "Email does not exist.";
                    return serviceResponse;
                }
                if (!HashPasswordHelper.VerifyPassword(authLoginDTO.password, existingUser.password))
                {
                    serviceResponse.statusCode = EHttpType.Unauthorized;
                    serviceResponse.success = false;
                    serviceResponse.message = "Invalid password.";
                    return serviceResponse;
                }
                string? token = await _jWTHelper.GenerateJWTToken(existingUser.id, existingUser.roleCode, DateTime.UtcNow.AddMinutes(10));
                serviceResponse.success = true;
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.message = "Login successful.";
                serviceResponse.data = new CredentialDTO { access_token = token };
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }
    }
}
