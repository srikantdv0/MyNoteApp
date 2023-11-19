using System;
using Microsoft.EntityFrameworkCore;
using Notes.Entities;

namespace Notes.DbContexts
{
	public class NoteContext : DbContext
	{
		public DbSet<Note> Notes { get; set; } = null!;
		public DbSet<SharedNote> SharedNotes { get; set; } = null!;
		public DbSet<Permission> Permissions { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Otp> Otps { get; set; } = null!;

		public NoteContext(DbContextOptions<NoteContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Permission>()
				.HasData(
				   new Permission("R")
				   {
					   Id = 1
				   },
				   new Permission("RW")
				   {
					   Id = 2
				   }

				);

			modelBuilder.Entity<User>()
				.HasData(
				   new User("srikantdv0@gmail.com", "Password123","SrikantfromGmail")
				   {
					   Id = 1,
					   CreatedDTS = DateTime.UtcNow
				   },
				   new User("srikant.yadav@gmail.com", "Srikant@123","SrikantfromWf")
				   {
					   Id = 2,
					   CreatedDTS = DateTime.UtcNow
				   }
				); ;
        }
	}
}

