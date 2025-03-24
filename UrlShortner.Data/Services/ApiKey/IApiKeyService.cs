namespace UrlShortner.Data.Services.ApiKey
{
    public interface IApiKeyService
    {
        Task<string> GetUserApiKey(string email);
        Task<string> GenerateApiKeyAsync(string email);
        Task<string> RevokeApiKeyAsync(string email);
    }
}
