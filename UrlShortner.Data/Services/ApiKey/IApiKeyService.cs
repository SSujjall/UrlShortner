namespace UrlShortner.Data.Services.ApiKey
{
    public interface IApiKeyService
    {
        Task<string> GenerateApiKeyAsync(string email);
        Task RevokeApiKeyAsync(string email);
    }
}
