using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Common
{
    public class CloudinaryDTO
    {
        [Required]
        public string path { get; set; }
        [Required]
        public string publicId { get; set; }

    }
}
