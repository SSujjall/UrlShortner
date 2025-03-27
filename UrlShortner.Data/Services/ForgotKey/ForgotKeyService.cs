using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Helpers;
using UrlShortner.Data.Repositories.ForgotKey;
using UrlShortner.Data.Repositories.User;
using UrlShortner.Data.Services.ApiKey;

namespace UrlShortner.Data.Services.ForgotKey
{
    public class ForgotKeyService : IForgotKeyService
    {
        private readonly IForgotKeyRepository _fpRepo;
        private readonly IUserRepository _userRepo;
        private readonly IApiKeyService _apiKeyService;

        public ForgotKeyService(IForgotKeyRepository repository, IUserRepository userRepo,
            IApiKeyService apiKeyService)
        {
            _fpRepo = repository;
            _userRepo = userRepo;
            _apiKeyService = apiKeyService;
        }

        public async Task<string> RequestOtp(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            string otp = OtpHelper.GenerateOtp();
            await _fpRepo.SaveOtpInDb(user.UserId, user.Email, otp);
            return otp;
        }

        public async Task<string> VerifyOtpAndResetKey(string email, string otp)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var userOtp = await _fpRepo.GetLatestOtp(email);
            if (userOtp == null)
            {
                return null;
            }

            if (userOtp.OtpCode != otp)
            {
                return null;
            }
            if (userOtp.OtpExpiryDate < DateTime.UtcNow)
            {
                return "Otp expired";
            }

            var newKey = await _apiKeyService.GenerateApiKeyAsync(email, true);
            if (string.IsNullOrEmpty(newKey))
            {
                return null;
            }

            userOtp.OtpUsed = true;
            await _fpRepo.SetOtpToUsed(userOtp);
            return newKey;
        }
    }
}
