using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IJwtRepositories
    {
        Task<JWT> AddJwtAsync(JWT jwt);
        Task<JWT> GetJwtByUserId(int id);
        Task<JWT> UpdateJwtAsync(JWT jwt);
        Task<bool> IsTokenExpiredAsync(JWT jwt);
    }
}
