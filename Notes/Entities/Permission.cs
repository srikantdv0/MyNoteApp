using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Entities
{
	public class Permission
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		[Required]
		public string Description { get; set; }
		public Permission(string description)
		{
			Description = description;
		}
	}
}

