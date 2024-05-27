using Microsoft.EntityFrameworkCore;
using System.Data;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class AuthRepositories : IAuthRepositories
    {
        private readonly ApplicationDbContext _context;
        public AuthRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> getByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<User> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
