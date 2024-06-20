using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Property
{
    public class GetPropertyDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public long description { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public string avatar { get; set; }
        [Required]
        public int category_id { get; set; }
        [Required]
        public int seller_id { get; set; }
    }
}
