using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Entities
{
	public class User
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[MaxLength(15)]
		public string Password { get; set; }
		[Required]
		public DateTime CreatedDTS { get; set; }
		[Required]
		public bool IsActive { get; set; } = true;
		//[Required]
		//public bool IsEmailVerified { get; set; } = false;
        public User(string email, string password, string name)
		{
			Email = email;
			Password = password;
			Name = name;
		}

	}
}

