using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortner.Data.Models.Requests
{
    public class ShortenUrlRequest
    {
        public string OriginalUrl { get; set; }
    }
}
