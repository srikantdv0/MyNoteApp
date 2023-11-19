using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
    public class UserProfileDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}


