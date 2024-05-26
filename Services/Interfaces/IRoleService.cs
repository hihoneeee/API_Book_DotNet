using TestWebAPI.DTOs.Role;
using TestWebAPI.Models;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResponse<AddRoleDTO>> AddRoleAsync(AddRoleDTO roleDTO);
        Task<ServiceResponse<List<RoleDTO>>> GetAllRoles();
        Task<ServiceResponse<RoleDTO>> GetRolesById(int id);
        Task<ServiceResponse<RoleDTO>> UpdateRoleAsync(int id, AddRoleDTO roleDTO);
        Task<ServiceResponse<RoleDTO>> DeleteRoleAsync(int id);

    }
}
