using TestWebAPI.DTOs.Property;
using TestWebAPI.Models;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IPropertyHasDetailServices
    {
        Task<ServiceResponse<PropertyHasDetailDTO>> CreateDetailAsync(int propertyId, PropertyHasDetailDTO propertyHasDetailDTO);
        Task<ServiceResponse<PropertyHasDetailDTO>> UpdateDetailAsync(int id, PropertyHasDetailDTO propertyHasDetailDTO);
        Task<ServiceResponse<PropertyHasDetail>> DeletePropertyAsync(int id);
    }
}
