using UrlShortner.Data.Filters;

namespace UrlShortner.Data.Models.Requests
{
    public class ApiKeyRequest
    {
        [Gmail]
        public string Email { get; set; }
    }
}
