using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public int CategoryId { get; set; } // Foreign key
        [ForeignKey("CategoryId")]
        public required Category Category { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public DateTime Updated { get; set; }
    }
}
