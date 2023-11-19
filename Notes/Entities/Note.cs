using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Entities
{
	public class Note
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		[MaxLength(10000)]
		public string Content { get; set; } = string.Empty;
		[Required]
		public DateTime CreatedDTS { get; set; }

		[ForeignKey("CreatorId")]
		public User? User { get; set; }
		public int CreatorId { get; set; }

		[Required]
		public string CreatorName { get; set; } = string.Empty;
      
        public int? ModifiedById { get; set; }

        public string? ModifiedByName { get; set; }

        public DateTime? ModifiedDTS { get; set; }

        public ICollection<SharedNote> SharedNote { get; set; }
                        = new List<SharedNote>();

        public Note(string title)
		{
			Title = title;
		}
	}
}

