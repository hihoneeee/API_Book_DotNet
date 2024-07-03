using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPropertyHasDetailRepositories
    {
        Task<PropertyHasDetail> CreatePropertyAsync(PropertyHasDetail propertyHasDetail);
        Task<PropertyHasDetail> GetPropertyHasDetailByPropertyIdAsync(int id);
    }
}
