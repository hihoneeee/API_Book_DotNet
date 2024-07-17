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

        public async Task<User> GetCurrentAsync(int id)
        {
            return await _context.Users
                .Include(u => u.User_Medias)
                .FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task<User> UpdateProfileUserAsync(User oldProfile, User newProfile)
        {
            oldProfile.first_name = newProfile.first_name;
            oldProfile.last_name = newProfile.last_name;
            oldProfile.address = newProfile.address;

            await _context.SaveChangesAsync();
            return oldProfile;
        }
        public async Task<User> UpdateAvatarUserAsync(User oldProfile, User newProfile)
        {
            oldProfile.avatar = newProfile.avatar;
            await _context.SaveChangesAsync();
            return oldProfile;
        }
    }
}
