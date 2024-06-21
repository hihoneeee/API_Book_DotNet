using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
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
        public int category_id { get; set; }
        [ForeignKey("category_id")]
        public virtual Category category { get; set; }

        public statusEnum status;
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
        public virtual ICollection<Nofication>? Nofications { get; set; }
        public virtual ICollection<Evaluate>? Evaluates { get; set; }
        public virtual ICollection<Contract>? Contracts { get; set; }
        public virtual ICollection<Submission>? Submissions { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<PropertyHasDetail>? PropertyHasDetails { get; set; }
    }
}
