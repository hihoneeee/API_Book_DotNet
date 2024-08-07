﻿using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPermisstionRepositories
    {
        Task<Permission> CreatePermissionAsyn(Permission permission);
        Task<List<Permission>> GetAllPermissionAsyn();
        Task<Permission> GetPermissionByValueAsyn(string value);
        Task<Permission> GetPermissionByIdAsyn(int id);
        Task<Permission> UpdatePermissionAsyn(Permission permission);
        Task<Object> DeletePermissionAsyn(Permission permission);
        Task<Permission> GetPermissionByCodeAsyn(string code);
    }
}
