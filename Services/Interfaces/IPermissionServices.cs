using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IPermissionServices
    {
        Task<ServiceResponse<AddPermissionDTO>> CreatePermissionAsyn(AddPermissionDTO permisstionDTO);
        Task<ServiceResponse<PermisstionDTO>> UpdatePermissionAsyn(int id, AddPermissionDTO permisstion);
        Task<ServiceResponse<PermisstionDTO>> DeletePermissionAsyn(int id);
        Task<ServiceResponse<List<PermisstionDTO>>> GetAllPermissionAsyn();
    }
}
