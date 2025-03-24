using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortner.Data.Models
{
    public class ApiKey
    {
        [Key]
        public string KeyId { get; set; } = Guid.NewGuid().ToString();
        public string ApiKeyHash { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ApiUser User { get; set; }
    }
}
