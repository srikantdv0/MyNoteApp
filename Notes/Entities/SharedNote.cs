using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Entities
{
	public class SharedNote
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ForeignKey("NoteId")]
		public Note? Note { get; set; }
		public int NoteId { get; set; }

		[ForeignKey("SharedToId")]
		public User? User { get; set; }
		public int SharedToId { get; set; }

        [ForeignKey("PermissionId")]
		public Permission? Permission { get; set; }
        public int PermissionId { get; set; }
	}
}

