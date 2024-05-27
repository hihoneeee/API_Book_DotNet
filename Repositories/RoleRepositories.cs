using Microsoft.EntityFrameworkCore;
using System.Data;
using TestWebAPI.Data;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
namespace TestWebAPI.Repositories
{
    public class RoleRepositories : IRoleRepositories
    {
        private readonly ApplicationDbContext _context;
        public RoleRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Role> GetRoleByValueAsync(string value)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.value == value);
        }
        public async Task<Role> AddRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;

        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }


        public async Task<Role> GetRolesById(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<object> DeleteRoleAsync(Role role)
        {
            _context.Roles!.Remove(role);
            return await _context.SaveChangesAsync();
        }

        public async Task<Role> UpdateRoleAsync(Role oldRole, Role newRole)
        {
            oldRole.value = newRole.value;
            await _context.SaveChangesAsync();
            return oldRole;
        }
    }
}
