﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
	public class SharedNoteContentDto
	{
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(10000)]
        public string? Content { get; set; }
        [Required]
        public int PermissionId { get; set; }
    }
}

