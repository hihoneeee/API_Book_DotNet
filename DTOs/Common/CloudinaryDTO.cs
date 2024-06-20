using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Common
{
    public class CloudinaryDTO
    {
        [Required]
        public IFormFile file { get; set; }
    }
}
