using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPropertyRepositories
    {
        Task<List<Property>> GetAllPropertyAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task<Property> CreatePropertyAsync(Property property);
        Task<Property> UpdatePropertyAsync(Property oldProperty, Property newProperty);
        Task<object> DeletePropertyAsync(Property property);

    }
}
