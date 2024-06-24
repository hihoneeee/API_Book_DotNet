using TestWebAPI.DTOs.Property;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IPropertyServices
    {
        Task<ServiceResponse<List<PropertyDTO>>> GetAllPropertyAsync();
        Task<ServiceResponse<PropertyDTO>> CreatePropertyAsync(PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO, string path, string publicId);
        Task<ServiceResponse<PropertyDTO>> UpdatePropertyAsync(int id, PropertyDTO propertyDTO);
        Task<ServiceResponse<PropertyDTO>> DeletePropertyAsync(int id);

    }
}
