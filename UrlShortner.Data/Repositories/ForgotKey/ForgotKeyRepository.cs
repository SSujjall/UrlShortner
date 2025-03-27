using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Repositories.ForgotKey
{
    public class ForgotKeyRepository : IForgotKeyRepository
    {
        private readonly AppDbContext _context;
        public ForgotKeyRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task SaveOtpInDb(string userId, string email, string otp)
        {
            var forgotKey = new UserOtp
            {
                UserId = userId,
                Email = email,
                OtpCode = otp,
                OtpExpiryDate = DateTime.UtcNow.AddMinutes(5)
            };
            await _context.UserOtps.AddAsync(forgotKey);
            await _context.SaveChangesAsync();
        }

        public async Task<UserOtp> GetLatestOtp(string email)
        {
            var otp = await _context.UserOtps
                                .Where(x => x.Email == email && x.OtpUsed == false)
                                .OrderByDescending(x => x.OtpExpiryDate)
                                .FirstOrDefaultAsync();

            return otp;
        }

        public async Task<bool> SetOtpToUsed(UserOtp model)
        {
            try
            {
                _context.UserOtps.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
