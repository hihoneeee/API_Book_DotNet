using System.ComponentModel.DataAnnotations;
using TestWebAPI.Helpers;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthLoginDTO
    {
        [RegularExpression(RegexUtilities.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        public string email { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string password { get; set; }
    }
}
