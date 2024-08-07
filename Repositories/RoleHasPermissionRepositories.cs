using Microsoft.EntityFrameworkCore;
using System.Data;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class RoleHasPermissionRepositories : IRoleHasPermissionRepositories
    {

        private readonly ApplicationDbContext _context;
        public RoleHasPermissionRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role_Permission> AssignPermissionAsync(Role_Permission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
            return rolePermission;
        }

        public async Task RemovePermissionAsync(Role_Permission rolePermission)
        {
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Role_Permission>> GetPermissionsByRoleCodeAsync(string roleCode)
        {
            return await _context.RolePermissions.Where(rp => rp.codeRole == roleCode).ToListAsync();
        }

        public async Task<Role_Permission> GetRoleByCodeAsync(string roleCode)
        {
            return await _context.RolePermissions.FirstOrDefaultAsync(p => p.codeRole == roleCode);
        }

        public async Task<bool> CheckRoleHasPermissonAsync(string codeRole, string codePermisson)
        {
            return await _context.RolePermissions.AnyAsync(rp => rp.codeRole == codeRole && rp.codePermission == codePermisson);
        }
        public async Task<List<string>> GetRolePermissionsAsync(string roleCode)
        {
            var userPermissions = await _context.RolePermissions
                                    .Where(rp => rp.codeRole == roleCode)
                                    .Select(rp => rp.Permission.value)
                                    .ToListAsync();
            return userPermissions;
        }
    }
}
