namespace TestWebAPI.Helpers
{
    public class RegexUtilities
    {
        public const string PHONE_NUMBER = "([84|0])([3|5|7|8|9])+([0-9]{8})";
        public const string EMAIL = @"^[\w-\.]+@gmail\.com$";
    }
}
