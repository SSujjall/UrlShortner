using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortner.Data.Models
{
    public class UserOtp
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("User")]
        public string UserId { get; set; }

        public string Email { get; set; }
        public string OtpCode { get; set; }
        public DateTime OtpExpiryDate { get; set; }
        public bool OtpUsed { get; set; } = false;

        // Navigation property
        public ApiUser User { get; set; }
    }
}
