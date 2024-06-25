using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Property
{
    public class GetPropertyDTO
    { 
        [Required]
        public int id { get; set; }
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
        [Required]
        public statusEnum status { get; set; }
        [Required]
        public DateTime createdAt { get; set; } 
        [Required]
        public DateTime updatedAt { get; set; }
        // public List<PropertyHasDetailDTO> propertyHasDetails { get; set; }

    }
}
