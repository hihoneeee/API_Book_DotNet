﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Nofication
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User user { get; set; }
        public int property_id { get; set; }
        [ForeignKey("property_id")]
        public virtual Property property { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
