using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public long description { get; set; }
        [Required]
        public string avatar {  get; set; }
        public DateTime createAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updateAt { get; set; }
        public virtual ICollection<Property>? Properties { get; set; }

    }
}
