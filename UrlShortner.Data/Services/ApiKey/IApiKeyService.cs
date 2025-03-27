namespace UrlShortner.Data.Services.ApiKey
{
    public interface IApiKeyService
    {
        Task<string> GetUserApiKey(string email);
        Task<string> GenerateApiKeyAsync(string email, bool isFp = false);
        Task<string> RevokeApiKeyAsync(string email);
    }
}
