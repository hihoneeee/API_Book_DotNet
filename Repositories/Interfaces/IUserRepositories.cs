using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IUserRepositories
    {   
        Task<User> GetCurrentAsync(int id);
    }
}
