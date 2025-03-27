using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Models;

namespace UrlShortner.Data.Repositories.ForgotKey
{
    public interface IForgotKeyRepository
    {
        Task SaveOtpInDb(string userId, string email, string otp);
        Task<UserOtp> GetLatestOtp(string email);
        Task<bool> SetOtpToUsed(UserOtp model);
    }
}
