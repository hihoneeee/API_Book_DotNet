using TestWebAPI.DTOs.Role;
using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IRoleRepositories
    {
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRolesById(int id);
        Task<Role> GetRoleByValueAsync(string value);
        Task<Role> AddRoleAsync(Role role);
        Task<object> DeleteRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(Role role);
        Task<Role> GetRoleByCodeAsyn(string code);
    }
}
