﻿using System.ComponentModel.DataAnnotations;
using TestWebAPI.DTOs.User;

namespace TestWebAPI.DTOs.ChatHub
{
    public class MessageDTO
    {
        [Required]
        public string content { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public int conversationId { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}
