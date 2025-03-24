using UrlShortner.Data.Extensions;
using UrlShortner.Data.Repositories.ApiKey;
using UrlShortner.Data.Repositories.User;

namespace UrlShortner.Data.Services.ApiKey
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository _apiKeyRepo;
        private readonly IUserRepository _userRepo;

        public ApiKeyService(IApiKeyRepository apiKeyRepo, IUserRepository userRepo)
        {
            _apiKeyRepo = apiKeyRepo;
            _userRepo = userRepo;
        }

        public async Task<string> GenerateApiKeyAsync(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new Models.ApiUser { Email = email };
                await _userRepo.AddUserAsync(user);
            }

            if(user.ApiKey != null)
            {
                if (user.ApiKey.IsActive)
                {
                    return "Cannot create new api key unless old one is revoked.";
                }
            }

            var newKey = await _apiKeyRepo.GenerateApiKeyAsync(user.UserId);
            return newKey.ApiKeyHash.Decrypt();
        }

        public async Task<string> GetUserApiKey(string email)
        {
            var apiKey = await _apiKeyRepo.GetUserApiKey(email);
            return apiKey;
        }

        public async Task<string> RevokeApiKeyAsync(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user.ApiKey.IsActive)
            {
                await _apiKeyRepo.RevokeApiKeyAsync(user.UserId);
                return "Api Key Revoked";
            }

            return "No active api key found.";
        }
    }
}
