using TestWebAPI.DTOs.User;

namespace TestWebAPI.Middlewares
{
    public interface IJWTHelper
    {
        Task<string> GenerateJWTToken(int id, string roleCode, DateTime expire);
    }
}
