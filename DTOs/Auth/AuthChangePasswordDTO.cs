using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthChangePasswordDTO
    {
        [JsonIgnore]
        public int id { get; set; }
        [Required(ErrorMessage = "The old password field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "The new password field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "The new password field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string enterPassword { get; set; }
    }
}
