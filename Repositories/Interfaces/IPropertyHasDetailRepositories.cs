using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPropertyHasDetailRepositories
    {
        Task<PropertyHasDetail> CreateDetailyAsync(PropertyHasDetail propertyHasDetail);
        Task<PropertyHasDetail> GetDetailByIdAsync(int id);
        Task<PropertyHasDetail> UpdateDetailyAsync(PropertyHasDetail oldPropertyHasDetail, PropertyHasDetail newPropertyHasDetail);
        Task<object> DeleteDetailAsync(PropertyHasDetail propertyHasDetail);
    }
}
