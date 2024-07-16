using AutoMapper;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Property;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepositories _cateRepo;
        private readonly ICloudinaryServices _cloudinaryServices;
        public CategoryServices(IMapper mapper, ICategoryRepositories cateRepo, ICloudinaryServices cloudinaryServices)
        {
            _mapper = mapper;
            _cateRepo = cateRepo;
            _cloudinaryServices = cloudinaryServices;
        }
        public async Task<ServiceResponse<CategoryDTO>> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            string publicId = null;

            try
            {
                var existingCate = await _cateRepo.GetCategoryByTitleAsync(categoryDTO.title);
                if (existingCate != null)
                {
                    serviceResponse.SetExisting("Category");
                    return serviceResponse;
                }

                var category = _mapper.Map<Category>(categoryDTO);

                var avatarUploadResult = await _cloudinaryServices.UploadImage(categoryDTO.avatar);
                if (avatarUploadResult == null || string.IsNullOrEmpty(avatarUploadResult.Url.ToString()))
                {
                    serviceResponse.SetError("Avatar upload failed");
                    return serviceResponse;
                }

                category.avatar = avatarUploadResult.Url.ToString();
                publicId = avatarUploadResult.PublicId;

                var addCategory = await _cateRepo.CreateCategoryAsync(category);
                serviceResponse.SetSuccess("Category created successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError($"An unexpected error occurred: {ex.Message}");                
                await _cloudinaryServices.DeleteImage(publicId);
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetCategoryDTO>>> GetCategoriesAsync()
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryDTO>>();
            try
            {
                var categories = await _cateRepo.GetCategoryAllAsync();
                if (categories == null)
                {
                    serviceResponse.SetNotFound("List Categories");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<List<GetCategoryDTO>>(categories);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryDTO>> GetCategoryByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<GetCategoryDTO>();
            try
            {
                var category = await _cateRepo.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    serviceResponse.SetNotFound("Category");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<GetCategoryDTO>(category);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<CategoryDTO>> UpdateCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            string publicId = null;
            try
            {
                var checkExist = await _cateRepo.GetCategoryByIdAsync(id);
                if (checkExist == null)
                {
                    serviceResponse.SetNotFound("Category");
                    return serviceResponse;
                }
                var CheckValue = await _cateRepo.GetCategoryByTitleAsync(categoryDTO.title);
                if (CheckValue != null)
                {
                    serviceResponse.SetExisting("Category");
                    return serviceResponse;
                }
                var oldImagePublicId = _cloudinaryServices.ExtractPublicIdFromUrl(checkExist.avatar);
                await _cloudinaryServices.DeleteImage(oldImagePublicId);
                var category = _mapper.Map<Category>(categoryDTO);
                var avatarUploadResult = await _cloudinaryServices.UploadImage(categoryDTO.avatar);
                if (avatarUploadResult == null || string.IsNullOrEmpty(avatarUploadResult.Url.ToString()))
                {
                    serviceResponse.SetError("Avatar upload failed");
                    return serviceResponse;
                }
                category.avatar = avatarUploadResult.Url.ToString();
                publicId = avatarUploadResult.PublicId;
                var updatedCategory = await _cateRepo.UpdateCategoryAsync(checkExist, category);
                serviceResponse.SetSuccess("Category updated successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                await _cloudinaryServices.DeleteImage(publicId);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<CategoryDTO>> DeleteCategoryAsync(int id)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            try
            {
                var checkExist = await _cateRepo.GetCategoryByIdAsync(id);
                if (checkExist == null)
                {
                    serviceResponse.SetNotFound("Category");
                    return serviceResponse;
                }
                var publicId = _cloudinaryServices.ExtractPublicIdFromUrl(checkExist.avatar);
                await _cloudinaryServices.DeleteImage(publicId);
                await _cateRepo.DeleteCategoryAsync(checkExist);
                serviceResponse.SetSuccess("Category deleted successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
