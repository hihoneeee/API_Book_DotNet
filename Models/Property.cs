using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
public enum typeEnum
{
    pending, 
    cancel,
    available
}  
public enum statusEnum 
{
   sale,
   rental
};

namespace TestWebAPI.Models
{
    public class Property
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public long description { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public int bedroom;
        [Required]
        public int bathroom;
        [Required]
        public int yearBuild;
        [Required]
        public int size;
        public bool isAvailable { get; set; }
        public Property()
        {
            isAvailable = true;
        }
        public int owner { get; set; }
        [ForeignKey("owner")]
        public virtual User user { get; set; }

        public int category_id { get; set; }
        [ForeignKey("category_id")]
        public virtual Category category { get; set; }
        public typeEnum type;

        public statusEnum status;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; }
        public virtual ICollection<Nofication>? Nofications { get; set; }

    }
}
