using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum statusEnum
{
    available = 1,
    pending = 2,
    complete = 3
}

namespace TestWebAPI.Models
{
    public class PropertyHasDetail
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string province { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public List<string> images { get; set; }
        [Required]
        public string address { get; set; }
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
        [ForeignKey("sellerId")]
        public virtual User seller { get; set; }
        [Required]
        public int propertyId { get; set; }
        [ForeignKey("propertyId")]
        public virtual Property property { get; set; }
        [Required]
        public statusEnum status { get; set; } = statusEnum.available;
    }
}
