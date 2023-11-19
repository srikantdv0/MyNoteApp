using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Entities
{
	public class Otp
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MinLength(6)]
		[MaxLength(6)]
		public int Code { get; set; }
		[Required]
		[EmailAddress]
		public string EmailId { get; set; } = string.Empty;
		[Required]
		public DateTime ValidTill { get; set; }
		[Required]
		public bool IsUsed { get; set; } = false;

	}
}

