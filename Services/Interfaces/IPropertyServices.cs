using TestWebAPI.DTOs.Property;
using TestWebAPI.Response;
using TestWebAPI.Settings;

namespace TestWebAPI.Services.Interfaces
{
    public interface IPropertyServices
    {
        Task<ServiceResponse<List<GetPropertyDTO>>> GetPropertiesAsync(QueryParamsSetting queryParams);
        Task<ServiceResponse<PropertyDTO>> CreatePropertyAsync(PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO);
        Task<ServiceResponse<PropertyDTO>> UpdatePropertyAsync(int id, PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO);
        Task<ServiceResponse<PropertyDTO>> DeletePropertyAsync(int id);

    }
}
