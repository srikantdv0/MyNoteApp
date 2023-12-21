using System;
using Notes.DbContexts;
using Notes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Notes.Services
{
	public class UserRepository: IUserRepository
	{
        private readonly NoteContext _context;

        public UserRepository(NoteContext noteContext)
		{
            _context = noteContext ?? throw new ArgumentNullException(nameof(noteContext));
        }

        public async Task CreateAccountAsync(User user)
        {
            await _context.AddAsync(user);
        }

        public void DisableAccount(User user)
        {
            _context.Update(user);
        }

        public async Task<User?> GetUserAsync(string emailAddress)
        {
            return await  _context.Users.Where(e => e.Email == emailAddress)
                .SingleOrDefaultAsync();
                    
        }

        public async Task<IEnumerable<User?>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();

        }

        public void ResetPasswordAsync(User user)
        {
            _context.Update(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task AddOtpAsync(Otp otp)
        {
            await ExpireOtpsAsync(otp.EmailId);
            await _context.AddAsync(otp);
        }

        public async Task<int> GetOtpAsync(string emailAddress)
        {
            var res = await _context.Otps.Where(o => o.EmailId == emailAddress && o.IsUsed == false && o.ValidTill >= DateTime.UtcNow).FirstOrDefaultAsync();
            if (res != null)
                return res.Code;
            else
                return 0;
        }

        public async Task<IEnumerable<Otp>> GetOtpsAsync()
        {
            return await _context.Otps.ToListAsync();
        }

        private async Task ExpireOtpsAsync(string emailAddress)
        {
            var otps = await _context.Otps.Where(o => o.EmailId == emailAddress).ToListAsync();
            if (otps.Count > 0)
            {
                otps.ForEach(o => o.IsUsed = true);
            }
        }
    }
}

