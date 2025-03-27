using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortner.Data.Services.ForgotKey
{
    public interface IForgotKeyService
    {
        Task<string> RequestOtp(string email);
        Task<string> VerifyOtpAndResetKey(string email, string otp);
    }
}
