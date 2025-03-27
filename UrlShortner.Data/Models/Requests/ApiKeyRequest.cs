using System.ComponentModel.DataAnnotations;
using UrlShortner.Data.Filters;

namespace UrlShortner.Data.Models.Requests
{
    public class ApiKeyRequest
    {
        [Gmail]
        [EmailAddress]
        public string Email { get; set; }
        public string? SecretKey { get; set; }
    }

    public class UserApiKeyRequest
    {
        [Gmail]
        [EmailAddress]
        public string Email { get; set; }
    }
}
