using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IAuthRepositories
    {
        Task<User> getByEmail(string email);
        Task<User> Register(User user);
    }
}
