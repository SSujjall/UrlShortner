using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Data.Models
{
    public class ApiUser
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }

        // Navigation property
        public ApiKey ApiKey { get; set; }
        public List<UserOtp> Otps { get; set; }

    }
}
