using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface ICategoryRepositories
    {
        Task<List<Category>> GetCategoryAllAsync();
        Task<Category> GetCategoryByTitleAsync(string value);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<object> DeleteCategoryAsync(Category category);
    }
}
