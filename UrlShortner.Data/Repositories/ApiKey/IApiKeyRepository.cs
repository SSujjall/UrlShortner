using UrlShortner.Data.Models;

namespace UrlShortner.Data.Repositories.ApiKey
{
    public interface IApiKeyRepository
    {
        Task<Models.ApiKey> GetApiKeyAsync(string apiKey);
        Task<Models.ApiKey> GenerateApiKeyAsync(string userId);
        Task RevokeApiKeyAsync(string userId);
    }
}
