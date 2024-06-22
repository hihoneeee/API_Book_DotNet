using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Property
{
    public class GetPropertyDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public string avatar { get; set; }
        [Required]
        public int categoryId { get; set; }
        [Required]
        public int sellerId { get; set; }
    }
}
