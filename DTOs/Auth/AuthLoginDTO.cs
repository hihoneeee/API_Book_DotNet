using System.ComponentModel.DataAnnotations;
using TestWebAPI.Helpers;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthLoginDTO
    {
        [RegularExpression(RegexUtilities.PHONE_NUMBER, ErrorMessage = "Phone number is not valid format!")]
        [StringLength(11, MinimumLength = 9)]
        [Required]
        public string phone { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        [Required]
        public string password { get; set; }
    }
}
