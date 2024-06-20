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
        public long images { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public int bedroom;
        [Required]
        public int bathroom;
        [Required]
        public int year_build;
        [Required]
        public int size;
        [Required]
        public int seller_id { get; set; }
        [ForeignKey("seller_id")]
        public virtual User seller { get; set; }
        [Required]
        public int property_id { get; set; }
        [ForeignKey("property_id")]
        public virtual Property property { get; set; }
        [Required]
        public typeEnum type;
    }
}
