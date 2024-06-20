﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Category
{
    public class CategoryDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public long description { get; set; }
    }
}
