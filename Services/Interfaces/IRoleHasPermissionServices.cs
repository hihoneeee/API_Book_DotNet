using TestWebAPI.DTOs.RoleHasPermission;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRoleHasPermissionServices
    {
        Task<ServiceResponse<AddRoleHasPermissionDTO>> AssignPermissionsAsync(AddRoleHasPermissionDTO addRoleHasPermissionDTO);
    }
}
