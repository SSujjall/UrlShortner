using MimeKit;

namespace UrlShortner.Data.Services.Email.EmailModel
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
