using System.ComponentModel.DataAnnotations;
using TestWebAPI.Helpers;
namespace TestWebAPI.DTOs.Category
{
    public class CategoryDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [ValidationFileImage]
        public IFormFile? avatar { get; set; }
    }
}
