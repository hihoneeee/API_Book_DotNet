using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IUserRepositories
    {   
        Task<User> GetCurrentAsync(int id);
        Task<User> getByEmail(string email);
        Task<User> UpdateProfileUserAsync(User user);
        Task<User> UpdateAvatarUserAsync(User oldProfile, User newProfile);
        Task<User> ChangeEmailUSerAsync(User oldEmail, string newEmail);
        Task<List<User>> GetUsersByIds(IEnumerable<int> userIds);
        Task<List<User>> GetUserInSystemAsync();
    }
}
