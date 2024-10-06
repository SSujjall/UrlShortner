using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortner.Data.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortLink { get; set; }
        public int NrOfClicks { get; set; }
        public string? UserId { get; set; }
        public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly DateUpdated { get; set; }

        // Navigation properties
        public User? User { get; set; }
    }
}
