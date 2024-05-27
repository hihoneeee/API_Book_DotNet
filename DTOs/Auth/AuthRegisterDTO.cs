using System.ComponentModel.DataAnnotations;
using TestWebAPI.Helpers;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthRegisterDTO
    {
        [RegularExpression(RegexUtilities.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        public string email { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string password { get; set; }
        public string roleCode { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        public string first_name { get; set; }
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")]
        public string last_name { get; set; }
    }
}
