using System;
using Notes.Entities;

namespace Notes.Services
{
	public interface IUserRepository
	{
		Task CreateAccountAsync(User user);
		void ResetPasswordAsync(User user);
		Task<User?> GetUserAsync(string emailAddress);
        Task<bool> SaveChangesAsync();
		Task<int> GetOtpAsync(string emailAddress);
		Task AddOtpAsync(Otp otp);
		Task<IEnumerable<Otp>> GetOtpsAsync();
        Task<IEnumerable<User?>> GetUsersAsync();
    }
}

