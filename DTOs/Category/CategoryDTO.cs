﻿using System.ComponentModel.DataAnnotations;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.DTOs.Property;

namespace TestWebAPI.DTOs.Category
{
    public class CategoryDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public long description { get; set; }
        [Required]
        public string avatar { get; set; }
        public List<GetPropertyDTO> dataProperties { get; set; }

    }
}
