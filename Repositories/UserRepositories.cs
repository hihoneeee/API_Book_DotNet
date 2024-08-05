using Humanizer;
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
        public async Task<User> getByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<User> GetCurrentAsync(int id)
        {
            return await _context.Users
                .Include(u => u.User_Medias)
                .FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task<List<User>> GetUsersByIds(IEnumerable<int> userIds)
        {
            return await _context.Users.Where(u => userIds.Contains(u.id)).ToListAsync();
        }

        public async Task<User> ChangeEmailUSerAsync(User oldEmail, string newEmail)
        {
            oldEmail.email = newEmail;
            await _context.SaveChangesAsync();
            return oldEmail;
        }

        public async Task<User> UpdateProfileUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateAvatarUserAsync(User oldProfile, User newProfile)
        {
            oldProfile.avatar = newProfile.avatar;
            await _context.SaveChangesAsync();
            return oldProfile;
        }
    }
}
