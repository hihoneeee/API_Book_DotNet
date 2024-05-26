using AutoMapper;
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
        private readonly IMapper _mapper;
        public RoleRepositories(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public IQueryable<Role> GetAllRoles()
        {
            return _context.Roles.AsQueryable();
        }


        public async Task<Role> GetRolesById(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role> DeleteRoleAsync(int id)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null)
            {
                return null;
            }
            _context.Roles!.Remove(existingRole);
            await _context.SaveChangesAsync();
            return existingRole;
        }

        public async Task<Role>UpdateRoleAsync(int id, Role role)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null)
            {
                return null;
            }
            existingRole.value = role.value;
            _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();
            return existingRole;
        }
    }
}
