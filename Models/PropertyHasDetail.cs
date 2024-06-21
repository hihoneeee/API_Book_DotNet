using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum typeEnum
{
    pending,
    cancel,
    available
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
        public string images { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public int bedroom { get; set; }
        [Required]
        public int bathroom { get; set; }
        [Required]
        public int year_build { get; set; }
        [Required]
        public int size { get; set; }
        [Required]
        public int seller_id { get; set; }
        [ForeignKey("seller_id")]
        public virtual User seller { get; set; }
        [Required]
        public int property_id { get; set; }
        [ForeignKey("property_id")]
        public virtual Property property { get; set; }
        [Required]
        public typeEnum type { get; set; }
    }
}
