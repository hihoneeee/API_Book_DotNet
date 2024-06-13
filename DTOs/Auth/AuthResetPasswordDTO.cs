using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TestWebAPI.Helpers;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthResetPasswordDTO
    {
        [Required(ErrorMessage = "The email field is required.")]
        [RegularExpression(RegexUtilities.EMAIL, ErrorMessage = "Email format is not Valid!")]
        public string email { get; set; }
        [JsonIgnore]
        public DateTime? passwordChangeAt { get; set; }
        [JsonIgnore]
        public string? passwordResetToken { get; set; }
        [JsonIgnore]
        public DateTime? passwordResetExpires { get; set; }
    }
}
