using TestWebAPI.DTOs.RoleHasPermission;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRoleHasPermissionServices
    {
        Task<ServiceResponse<RoleHasPermissionDTO>> AssignPermissionAsyn(string roleCode, string permissionCode, RoleHasPermissionDTO roleHasPermissionDTO);
    }
}
