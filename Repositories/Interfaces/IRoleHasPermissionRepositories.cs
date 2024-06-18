using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IRoleHasPermissionRepositories
    {
        Task<Role_Permission> AssignPermissionAsync(Role_Permission roleHasPermission);
        Task<bool> CheckRoleHasPermissonAsync(string codeRole, string codePermisson);
    }
}
