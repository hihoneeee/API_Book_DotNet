using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthResetPasswordDTO
    {
        [Required(ErrorMessage = "The email field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string password { get; set; }
    }
}
