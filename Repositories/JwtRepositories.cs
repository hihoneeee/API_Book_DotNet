using Microsoft.EntityFrameworkCore;
using System.Data;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class JwtRepositories : IJwtRepositories
    {
        private readonly ApplicationDbContext _context;
        public JwtRepositories(ApplicationDbContext context) {
            _context = context;

        }
        public async Task<JWT> GetJwtByUserId(int id)
        {
            return await _context.JWTs.FirstOrDefaultAsync(j => j.user_id == id);
        }

        public async Task<JWT> AddJwtAsync(JWT jwt)
        {
            _context.JWTs.Add(jwt);
            await _context.SaveChangesAsync();
            return jwt;
        }

        public async Task<bool> IsTokenExpiredAsync(JWT jwt)
        {
            return jwt.issued_date >= jwt.expired_date;
        }

        public async Task<JWT> UpdateJwtAsync(JWT jwt)
        {
            _context.JWTs.Update(jwt);
            await _context.SaveChangesAsync();
            return jwt;
        }

    }
}
