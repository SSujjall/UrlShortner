using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Services.Email.EmailModel;

namespace UrlShortner.Data.Services.Email
{
    public interface IEmailService
    {
        Task SendMail(EmailMessage message);
    }
}
