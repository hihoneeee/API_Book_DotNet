using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly ApplicationDbContext _context;

        public UserRepositories(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<User> GetCurrentAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.id == id);
        }
    }
}
