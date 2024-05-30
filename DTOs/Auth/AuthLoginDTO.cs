using System.ComponentModel.DataAnnotations;
using TestWebAPI.Helpers;

namespace TestWebAPI.DTOs.Auth
{
    public class AuthLoginDTO
    {
        public string email { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string password { get; set; }
    }
}
