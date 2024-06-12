using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPermisstionRepositories
    {
        Task<Permission> CreatePermissionAsyn(Permission permission);
        Task<List<Permission>> GetAllPermissionAsyn();
        Task<Permission> GetPermissionByValueAsyn(string value);
        Task<Permission> GetPermissionByIdAsyn(int id);
        Task<Permission> UpdatePermissionAsyn(Permission oldPer, Permission newPer);
        Task<Object> DeletePermissionAsyn(Permission permission);
    }
}
