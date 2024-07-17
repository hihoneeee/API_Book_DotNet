using AutoMapper;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.User;
using TestWebAPI.Helpers;
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

        public UserServices(IUserRepositories userRepo, IMapper mapper, ICloudinaryServices cloudinaryServices)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _cloudinaryServices = cloudinaryServices;

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
                var newProfile = _mapper.Map<User>(userDTO);
                var updatedCategory = await _userRepo.UpdateProfileUserAsync(existingUser, newProfile);
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
                serviceResponse.SetSuccess("Get current user successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");
                await _cloudinaryServices.DeleteImage(publicId);
            }
            return serviceResponse;
        }
    }
}
