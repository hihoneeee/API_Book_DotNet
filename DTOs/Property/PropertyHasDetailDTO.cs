using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Property
{
    public class PropertyHasDetailDTO
    {
        [Required]
        public string province { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string images { get; set; }
        [Required]
        public int bedroom { get; set; }
        [Required]
        public int bathroom { get; set; }
        [Required]
        public int yearBuild { get; set; }
        [Required]
        public int size { get; set; }
        [Required]
        public int sellerId { get; set; }
        [Required]
        public typeEnum Type { get; set; }
    }
}
