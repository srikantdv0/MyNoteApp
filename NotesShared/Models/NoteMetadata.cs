using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesShared.Models
{
	public class NoteMetadata
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDTS { get; set; }
        
        public DateTime? ModifiedDTS { get; set; }

        public int? ModifiedById { get; set; }
        public string? ModifiedByName { get; set; }

    }
}

