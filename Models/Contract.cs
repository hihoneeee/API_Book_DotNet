﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Contract
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string signatureBuyer { get; set; }
        [Required]
        public string signatureSeller { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public int offerId { get; set; }
        [ForeignKey("offerId")]
        public virtual Offer offer { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
