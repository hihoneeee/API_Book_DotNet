namespace TestWebAPI.Helpers.IHelpers
{
    public interface IHashPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
