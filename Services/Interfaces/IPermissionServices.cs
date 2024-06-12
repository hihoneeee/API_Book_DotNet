using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IPermissionServices
    {
        Task<ServiceResponse<PermisstionDTO>> CreatePermissionAsyn(PermisstionDTO permisstionDTO);
        Task<ServiceResponse<PermisstionDTO>> UpdatePermissionAsyn(int id, PermisstionDTO permisstion);
        Task<ServiceResponse<PermisstionDTO>> DeletePermissionAsyn(int id);
        Task<ServiceResponse<List<PermisstionDTO>>> GetAllPermissionAsyn();
    }
}
