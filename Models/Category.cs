using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public DateTime Updated { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
