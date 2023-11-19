using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
	public class SharedNoteUsersDto
	{
		[Required]
		public int UserId { get; set; }
		[Required]
		public string UserEmail { get; set; } = string.Empty;
		[Required]
		public string UserName { get; set; } = string.Empty;
		[Required]
		public string Permission { get; set; } = string.Empty;
	}
}

