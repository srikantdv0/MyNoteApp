using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{
	public class UserForCreationDto
	{
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		[MaxLength(15)]
		public string Password { get; set; } = string.Empty;
		[Required]
		[MaxLength(30)]
		public string Name { get; set; } = string.Empty;
	}
}

