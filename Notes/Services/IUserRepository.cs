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
		Task<int> GetOtp(string emailAddress);
		Task AddOtp(Otp otp);
		Task<IEnumerable<Otp>> GetOtps();
        Task<IEnumerable<User?>> GetUsersAsync();
    }
}

