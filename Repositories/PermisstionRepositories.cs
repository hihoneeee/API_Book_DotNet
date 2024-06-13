using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class PermisstionRepositories : IPermisstionRepositories
    {
        private readonly ApplicationDbContext _context;
        public PermisstionRepositories(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<List<Permission>> GetAllPermissionAsyn()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission> GetPermissionByValueAsyn(string value)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.value == value);
        }

        public async Task<Permission> GetPermissionByIdAsyn(int id)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<Permission> CreatePermissionAsyn(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<Permission> UpdatePermissionAsyn(Permission oldPer, Permission newPer)
        {
            oldPer.value = newPer.value;
            await _context.SaveChangesAsync();
            return oldPer;
        }

        public async Task<object> DeletePermissionAsyn(Permission permission)
        {
            _context.Permissions!.Remove(permission);
            return await _context.SaveChangesAsync();
        }

        public async Task<Permission> GetPermissionByCodeAsyn(string code)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.code == code);
        }

    }
}
