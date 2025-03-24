using UrlShortner.Data.Models;

namespace UrlShortner.Data.Repositories.ApiKey
{
    public interface IApiKeyRepository
    {
        Task<string> GetUserApiKey(string email);
        Task<Models.ApiKey> CheckApiKey(string apiKey);
        Task<Models.ApiKey> GenerateApiKeyAsync(string userId);
        Task RevokeApiKeyAsync(string userId);
    }
}
