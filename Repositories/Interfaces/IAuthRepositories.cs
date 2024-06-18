using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IAuthRepositories
    {
        Task<User> getByEmail(string email);
        Task<User> Register(User user);
        Task<User> InsertChangePasswordAsyn(User user);
        Task<User> FindPasswordResetTokenAsyn(string token);
        Task<User> ResetNewPasswordAsync(string newPassword, User user);
        Task<User> ChangePasswordAsync(string newPassword, User user);
    }
}
