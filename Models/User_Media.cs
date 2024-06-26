﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class User_Media
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string provider { get; set; }
        [Required]
        public string icon { get; set; }
        [Required]
        public string link { get; set; }
        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
