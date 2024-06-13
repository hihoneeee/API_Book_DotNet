using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IRoleHasPermissionRepositories
    {
        Task<Role_Permission> AssignPermissionAsyn(Role_Permission roleHasPermission);
  
    }
}
