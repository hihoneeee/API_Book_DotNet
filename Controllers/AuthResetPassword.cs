using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Controllers
{
    public class AuthResetPassword
    {
        [Required(ErrorMessage = "The email field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string password { get; set; }
    }
}
