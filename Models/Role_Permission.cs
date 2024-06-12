﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Role_Permission
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int codeRole { get; set; }

        [ForeignKey("codeRole")]
        public virtual Role Role { get; set; }

        [Required]
        public int codePermission { get; set; }

        [ForeignKey("codePermission")]
        public virtual Permission Permission { get; set; }

        [Required]
        public DateTime created { get; set; } = DateTime.Now;

        [Required]
        public DateTime updated { get; set; }
    }
}
