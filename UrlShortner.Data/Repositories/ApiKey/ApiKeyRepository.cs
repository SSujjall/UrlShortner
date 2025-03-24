using Microsoft.EntityFrameworkCore;
using UrlShortner.Data.Extensions;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Repositories.ApiKey
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly AppDbContext _dbContext;

        public ApiKeyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.ApiKey> GenerateApiKeyAsync(string userId)
        {
            var newKey = new Models.ApiKey
            {
                ApiKeyHash = (Guid.NewGuid().ToString("N")+Guid.NewGuid().ToString("N")).Encrypt(),
                UserId = userId
            };

            await _dbContext.ApiKeys.AddAsync(newKey);
            await _dbContext.SaveChangesAsync();
            return newKey;
        }

        public async Task<Models.ApiKey> CheckApiKey(string apiKey)
        {
            return await _dbContext.ApiKeys.Include(k => k.User)
            .FirstOrDefaultAsync(k => k.ApiKeyHash == apiKey.Encrypt() && k.IsActive);
        }

        public async Task RevokeApiKeyAsync(string userId)
        {
            var key = await _dbContext.ApiKeys.FirstOrDefaultAsync(k => k.UserId == userId);
            if (key != null) key.IsActive = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetUserApiKey(string email)
        {
            var res = await _dbContext.ApiKeys.FirstOrDefaultAsync(k => k.User.Email == email && k.IsActive);
            return res.ApiKeyHash.Decrypt();
        }
    }
}
