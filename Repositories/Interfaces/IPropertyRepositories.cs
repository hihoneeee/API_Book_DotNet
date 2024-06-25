using System.Dynamic;
using TestWebAPI.Models;
using TestWebAPI.Settings;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPropertyRepositories
    {
       Task<(List<ExpandoObject>, int)> GetPropertiesAsync(QueryParamsSetting queryParams);
        Task<Property> GetPropertyByIdAsync(int id);
        Task<Property> CreatePropertyAsync(Property property);
        Task<Property> UpdatePropertyAsync(Property oldProperty, Property newProperty);
        Task<object> DeletePropertyAsync(Property property);
    }
}
