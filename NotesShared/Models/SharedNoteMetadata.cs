using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
	public class SharedNoteMetadata
	{
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int PermissionId { get; set; }

        [Required]
        public DateTime CreatedDTS { get; set; }
        [Required]
        public int CreatorId { get; set; }
        [Required]
        public string CreatorName { get; set; } = string.Empty;

        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDTS { get; set; }
        public string? ModifiedByName { get; set; }

    }
}

