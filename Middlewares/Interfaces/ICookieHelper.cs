namespace TestWebAPI.Middlewares.Interfaces
{
    public interface ICookieHelper
    {
        Task GenerateCookie(int id, string phone, string email, string firstName, string lastName, string roleCode, string avatar, string roleName, DateTime expire);
        string GetUserRole();
        string GetUserAvatar();
        string GetUserFullName();
        string GetUserRoleName();

    }
}
