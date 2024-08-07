using Microsoft.EntityFrameworkCore;
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

        public async Task<User> getByPhoneAsync(string phone)
        {
            return await _context.Users.Include(u => u.Role).ThenInclude(r=>r.Role_Permissions).ThenInclude(rp=>rp.Permission)
                                 .Include(u => u.User_Medias)
                                 .FirstOrDefaultAsync(u => u.phone == phone);
        }

        public async Task<User> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> InsertChangePasswordAsyn(User user)
        {
            var token = Guid.NewGuid().ToString();
            user.passwordChangeAt = DateTime.UtcNow;
            user.passwordResetExpires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(15));
            user.passwordResetToken = token;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> FindPasswordResetTokenAsyn(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.passwordResetToken == token);
        }

        public async Task<User> ResetNewPasswordAsync(string newPassword, User user)
        {
           user.password = newPassword;
           await _context.SaveChangesAsync();
           return user;
        }
        public async Task<User> ChangePasswordAsync(string newPassword, User user)
        {
            user.password = newPassword;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
