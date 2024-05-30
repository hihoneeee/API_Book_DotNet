using TestWebAPI.DTOs.JWT;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IJwtService
    {
        Task InsertJWTToken(jwtDTO jwtDTO);
    }
}
