using AutoMapper;
using TestWebAPI.DTOs.Role;
using TestWebAPI.DTOs.User;
using TestWebAPI.Helpers;
using TestWebAPI.Helpers.IHelpers;
using TestWebAPI.Middlewares;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositories _userRepo;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IHashPasswordHelper _hashPasswordHelper;
        private readonly IJWTHelper _jWTHelper;
        public UserServices(IUserRepositories userRepo, IMapper mapper, ICloudinaryServices cloudinaryServices, IHashPasswordHelper hashPasswordHelper, IJWTHelper jWTHelper)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _cloudinaryServices = cloudinaryServices;
            _hashPasswordHelper = hashPasswordHelper;
            _jWTHelper = jWTHelper;
        }

        public async Task<ServiceResponse<GetUserDTO>> GetCurrentAsync(int id)
        {
            var serviceResponse = new ServiceResponse<GetUserDTO>();
            try
            {
                var existingUser = await _userRepo.GetCurrentAsync(id);
                if (existingUser == null)
                {
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<GetUserDTO>(existingUser);
                serviceResponse.SetSuccess("Get current user successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserDTO>> UpdateProfileUserAsync(int id, UserDTO userDTO)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            try
            {
                var existingUser = await _userRepo.GetCurrentAsync(id);
                if (existingUser == null)
                {
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                existingUser.first_name = userDTO.first_name;
                existingUser.last_name = userDTO.last_name;
                existingUser.address = userDTO.address;
                var updatedCategory = await _userRepo.UpdateProfileUserAsync(existingUser);
                serviceResponse.SetSuccess("Get current user successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<AvatarUserDTO>> UpdateAvatarUserAsync(int id, AvatarUserDTO avatarUserDTO)
        {
            var serviceResponse = new ServiceResponse<AvatarUserDTO>();
            string publicId = null;
            try
            {
                var existingUser = await _userRepo.GetCurrentAsync(id);
                if (existingUser == null)
                {
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                var oldImagePublicId = _cloudinaryServices.ExtractPublicIdFromUrl(existingUser.avatar);
                await _cloudinaryServices.DeleteImage(oldImagePublicId);
                var newProfile = _mapper.Map<User>(avatarUserDTO);
                var avatarUploadResult = await _cloudinaryServices.UploadImage(avatarUserDTO.avatar);
                if (avatarUploadResult == null || string.IsNullOrEmpty(avatarUploadResult.Url.ToString()))
                {
                    serviceResponse.SetError("Avatar upload failed");
                    return serviceResponse;
                }
                newProfile.avatar = avatarUploadResult.Url.ToString();
                publicId = avatarUploadResult.PublicId;
                var updatedCategory = await _userRepo.UpdateAvatarUserAsync(existingUser, newProfile);
                serviceResponse.SetSuccess("Change avatar successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");
                await _cloudinaryServices.DeleteImage(publicId);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserDTO>> ChangeEmailUserAsync (int id, EmailUSerDTO emailUSerDTO)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            try
            {
                var checkUser = await _userRepo.GetCurrentAsync(id);
                if (checkUser == null)
                {
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                var checkNewEmail = await _userRepo.getByEmail(emailUSerDTO.mewEmail);
                if (checkNewEmail != null) {
                    serviceResponse.SetExisting("Email");
                    return serviceResponse;
                }
                if (!_hashPasswordHelper.VerifyPassword(emailUSerDTO.currentPassword, checkUser.password))
                {
                    serviceResponse.SetUnauthorized("Password is wrong!");
                    return serviceResponse;
                }
                string accessToken = await _jWTHelper.GenerateJWTTokenForEmail(checkUser.id, emailUSerDTO.mewEmail, checkUser.email, DateTime.UtcNow.AddMinutes(10));
                serviceResponse.accessToken = accessToken;
                serviceResponse.SetSuccess("Send email succssefully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserDTO>> ConfirmChangeEmailUserAsync(string token)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            try
            {
                var tokenNewEmail =  _jWTHelper.GetNewEmailFromToken(token);
                var tokenOldEmail = _jWTHelper.GetOldEmailFromToken(token);
                var tokenUserId = _jWTHelper.GetUserIdFromToken(token);

                var checkUser = await _userRepo.GetCurrentAsync(tokenUserId);
                if (checkUser == null)
                {
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                var checkOldEmail = _userRepo.getByEmail(tokenOldEmail);
                if (checkOldEmail == null)
                {
                    serviceResponse.SetNotFound("Old email");
                    return serviceResponse;
                }
                var changeEmail = await _userRepo.ChangeEmailUSerAsync(checkUser, tokenNewEmail);
                serviceResponse.SetSuccess("Gmail change succssefully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserSystemDTO>>> GetUserInSystemAsync()
        {
            var serviceResponse = new ServiceResponse<List<GetUserSystemDTO>>();
            try
            {
                var users = await _userRepo.GetUserInSystemAsync();
                serviceResponse.data = _mapper.Map<List<GetUserSystemDTO>>(users);
                serviceResponse.SetSuccess("Gmail change succssefully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");

            }
            return serviceResponse;
        }
    }
}
