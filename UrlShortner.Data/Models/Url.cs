using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortner.Data.Models
{
    public class Url
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        [Required]
        public string ShortUrl { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Navigation property
        public ApiUser User { get; set; }
    }
}
