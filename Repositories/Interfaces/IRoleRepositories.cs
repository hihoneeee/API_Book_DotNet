using TestWebAPI.DTOs.Role;
using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IRoleRepositories
    {
        IQueryable<Role> GetAllRoles();
        Task<Role> GetRolesById(int id);
        Task<Role> GetRoleByValueAsync(string value);
        Task<Role> AddRoleAsync(Role role);
        Task<Role> DeleteRoleAsync(int id);
        Task<Role> UpdateRoleAsync(int id, Role role);
    }
}
