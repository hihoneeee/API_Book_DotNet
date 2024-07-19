using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IUserRepositories
    {   
        Task<User> GetCurrentAsync(int id);
        Task<User> getByEmail(string email);
        Task<User> UpdateProfileUserAsync(User oldProfile, User newProfile);
        Task<User> UpdateAvatarUserAsync(User oldProfile, User newProfile);
        Task<User> ChangeEmailUSerAsync(User oldEmail, string newEmail);
    }
}
