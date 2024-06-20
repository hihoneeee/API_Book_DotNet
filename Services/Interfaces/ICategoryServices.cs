using TestWebAPI.DTOs.Category;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface ICategoryServices
    {
        Task<ServiceResponse<List<CategoryDTO>>> GetCategoriesAsync();
        Task<ServiceResponse<CategoryDTO>> CreateCategoryAsync(CategoryDTO categoryDTO, string path, string publicId);
        Task<ServiceResponse<CategoryDTO>> UpdateCategoryAsync(int id, CategoryDTO categoryDTO);
        Task<ServiceResponse<CategoryDTO>> DeleteCategoryAsync(int id);
    }
}
