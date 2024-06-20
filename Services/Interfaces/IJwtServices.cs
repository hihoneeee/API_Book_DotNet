using TestWebAPI.DTOs.JWT;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IJwtServices
    {
        Task InsertJWTToken(jwtDTO jwtDTO);
    }
}
