﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public enum TypeEnum 
{
    sale = 1,
    rental = 2
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
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public virtual Category category { get; set; }
        [Required]
        public TypeEnum type { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
        public virtual ICollection<Contract>? Contracts { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual PropertyHasDetail PropertyHasDetail { get; set; }
    }
}
