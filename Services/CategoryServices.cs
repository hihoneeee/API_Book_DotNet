using AutoMapper;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IMapper _mapper;
        private ICategoryRepositories _cateRepo;

        public CategoryServices(IMapper mapper, ICategoryRepositories cateRepo)
        {
            _mapper = mapper;
            _cateRepo = cateRepo;
        }
        public async Task<ServiceResponse<CategoryDTO>> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            try
            {
                var existingCate = await _cateRepo.GetCategoryByTitleAsync(categoryDTO.title);
                if (existingCate != null)
                {
                    serviceResponse.SetExisting("Category");
                    return serviceResponse;
                }
                var category = _mapper.Map<Category>(categoryDTO);
                var addCategory = await _cateRepo.CreateCategoryAsync(category);
                serviceResponse.SetExisting("Category created successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CategoryDTO>>> GetCategoriesAsync()
        {
            var serviceResponse = new ServiceResponse<List<CategoryDTO>>();
            try
            {
                var categories = await _cateRepo.GetCategoryAllAsync();
                if (categories == null)
                {
                    serviceResponse.SetNotFound("List Categories");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<List<CategoryDTO>>(categories);
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
            try
            {
                var checkExist = await _cateRepo.GetCategoryByIdAsync(id);
                if (checkExist == null)
                {
                    serviceResponse.SetNotFound("Category");
                    return serviceResponse;
                }
                var CheckValue = await _cateRepo.GetCategoryByTitleAsync(categoryDTO.title);
                if (checkExist != null)
                {
                    serviceResponse.SetExisting("Category");
                    return serviceResponse;
                }
                var category = _mapper.Map<Category>(categoryDTO);
                var updatedRole = await _cateRepo.UpdateCategoryAsync(checkExist, category);
                serviceResponse.SetSuccess("Category updated successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
