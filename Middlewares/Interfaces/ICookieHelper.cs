using TestWebAPI.DTOs.User;

namespace TestWebAPI.Middlewares.Interfaces
{
    public interface ICookieHelper
    {
        Task GenerateCookie(GetUserDTO userDto, DateTime expire);
        string GetUserRole();
        string GetUserAvatar();
        string GetUserFullName();
        string GetUserRoleName();
        List<string> GetUserPermissions();
    }
}
