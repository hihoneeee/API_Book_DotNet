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

        public async Task<Role_Permission> AssignPermissionAsync(Role_Permission roleHasPermission)
        {
            _context.RolePermissions.Add(roleHasPermission);
            await _context.SaveChangesAsync();
            return roleHasPermission;
        }
        public async Task<bool> CheckRoleHasPermissonAsync(string codeRole, string codePermisson)
        {
            return await _context.RolePermissions.AnyAsync(rp => rp.codeRole == codeRole && rp.codePermission == codePermisson);
        }

    }
}
