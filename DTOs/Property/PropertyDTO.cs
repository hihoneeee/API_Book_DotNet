using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Property
{
    public class PropertyDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public IFormFile avatar { get; set; }
        [Required]
        public int categoryId { get; set; }
        [Required]
        public EnumStatus status { get; set; }
    }
}
