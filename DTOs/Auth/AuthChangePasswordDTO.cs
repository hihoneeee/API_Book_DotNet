using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthChangePasswordDTO
    {
        public string id { get; set; }
        [Required(ErrorMessage = "The old password field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "The new password field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string newPassword { get; set; }
    }
}
