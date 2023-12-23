using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
	public class NotesForCreationDto
	{
        [Required(ErrorMessage = "Note title can't be empty")]
        [MaxLength(20, ErrorMessage = "Title length can't exceed 20 characters")]
        public string Title { get; set; } = string.Empty;
        [MaxLength(10000, ErrorMessage = "Content length can't exceed 10000 characters")]
        public string? Content { get; set; }
    }
}

