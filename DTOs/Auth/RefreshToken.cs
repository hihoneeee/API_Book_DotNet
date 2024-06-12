using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Auth
{
    public class refreshToken
    {
        [Required(ErrorMessage = "The 'token' field is required.")]
        public required string token { get; set; }
    }
}
