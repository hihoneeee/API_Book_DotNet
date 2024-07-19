using System.ComponentModel.DataAnnotations;
using TestWebAPI.Helpers;

namespace TestWebAPI.DTOs.User
{
    public class EmailUSerDTO
    {
        [Required(ErrorMessage = "The email field is required.")]
        [RegularExpression(RegexUtilities.EMAIL, ErrorMessage = "Email format is not Valid!")]
        public string mewEmail { get; set; }
        [Required(ErrorMessage = "The password field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string currentPassword { get; set; }
    }
}
