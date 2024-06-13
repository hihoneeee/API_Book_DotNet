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

        public async Task<Role_Permission> AssignPermissionAsyn(Role_Permission roleHasPermission)
        {
            _context.RolePermissions.Add(roleHasPermission);
            await _context.SaveChangesAsync();
            return roleHasPermission;
        }

    }
}
