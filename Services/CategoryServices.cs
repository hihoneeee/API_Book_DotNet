using AutoMapper;
using Newtonsoft.Json;
using TestWebAPI.Configs;
using TestWebAPI.DTOs.Category;
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
        private readonly RedisCacheConfig _redisCacheConfig;
        public CategoryServices(IMapper mapper, ICategoryRepositories cateRepo, ICloudinaryServices cloudinaryServices, RedisCacheConfig redisCacheConfig)
        {
            _mapper = mapper;
            _cateRepo = cateRepo;
            _cloudinaryServices = cloudinaryServices;
            _redisCacheConfig = redisCacheConfig;
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
        //public async Task<ServiceResponse<List<GetCategoryDTO>>> GetCategoriesAsync()
        //{
        //    var serviceResponse = new ServiceResponse<List<GetCategoryDTO>>();
        //    try
        //    {
        //        var categories = await _cateRepo.GetCategoryAllAsync();
        //        if (categories == null)
        //        {
        //            serviceResponse.SetNotFound("List Categories");
        //            return serviceResponse;
        //        }
        //        serviceResponse.data = _mapper.Map<List<GetCategoryDTO>>(categories);
        //        serviceResponse.SetSuccess("Get successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse.SetError(ex.Message);
        //    }
        //    return serviceResponse;
        //}

        public async Task<ServiceResponse<List<GetCategoryDTO>>> GetCategoriesAsync()
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryDTO>>();
            try
            {
                // Generate cache key
                var cacheKey = "categories_all";

                // Check Redis cache
                var cachedData = await _redisCacheConfig.GetCacheValueAsync(cacheKey);

                List<GetCategoryDTO> categories;

                if (!string.IsNullOrEmpty(cachedData))
                {
                    // Deserialize cached data
                    categories = JsonConvert.DeserializeObject<List<GetCategoryDTO>>(cachedData);
                }
                else
                {
                    // Fetch from DB
                    var categoriesFromDb = await _cateRepo.GetCategoryAllAsync();
                    if (categoriesFromDb == null)
                    {
                        serviceResponse.SetNotFound("List Categories");
                        return serviceResponse;
                    }

                    categories = _mapper.Map<List<GetCategoryDTO>>(categoriesFromDb);

                    // Save to Redis cache
                    var serializedData = JsonConvert.SerializeObject(categories);
                    await _redisCacheConfig.SetCacheValueAsync(cacheKey, serializedData, TimeSpan.FromMinutes(60));
                }

                serviceResponse.data = categories;
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

                var checkValue = await _cateRepo.GetCategoryByTitleAsync(categoryDTO.title);
                if (checkValue != null && checkValue.id != id)
                {
                    serviceResponse.SetExisting("Category");
                    return serviceResponse;
                }

                checkExist.title = categoryDTO.title;
                checkExist.description = categoryDTO.description;

                if (categoryDTO.avatar != null)
                {
                    var oldImagePublicId = _cloudinaryServices.ExtractPublicIdFromUrl(checkExist.avatar);
                    await _cloudinaryServices.DeleteImage(oldImagePublicId);
                    var avatarUploadResult = await _cloudinaryServices.UploadImage(categoryDTO.avatar);
                    if (avatarUploadResult == null || string.IsNullOrEmpty(avatarUploadResult.Url.ToString()))
                    {
                        serviceResponse.SetError("Avatar upload failed");
                        return serviceResponse;
                    }
                    checkExist.avatar = avatarUploadResult.Url.ToString();
                    publicId = avatarUploadResult.PublicId;
                }

                checkExist.updateAt = DateTime.Now;

                await _cateRepo.UpdateCategoryAsync(checkExist);

                serviceResponse.SetSuccess("Category updated successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                if (publicId != null)
                {
                    await _cloudinaryServices.DeleteImage(publicId);
                }
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
