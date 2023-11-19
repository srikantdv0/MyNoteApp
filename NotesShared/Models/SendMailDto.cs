using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
    public class SendMailDto
    {
        [Required]
        public string ReciverName { get; set; } = string.Empty;
        [Required]
        public string ReciverEmail { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
    }
}

