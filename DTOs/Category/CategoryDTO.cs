using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Category
{
    public class CategoryDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        public IFormFile avatar { get; set; }
    }
}
