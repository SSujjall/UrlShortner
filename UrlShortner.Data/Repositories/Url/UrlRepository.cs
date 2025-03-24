using System.Text;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Repositories.Url
{
    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _dbContext;

        public UrlRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseModel> CreateShortenedUrlAsync(string userId,string originalUrl)
        {
            var shortUrl = GenerateShortUrl();
            var shortenedUrl = new Models.Url
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl,
                UserID = userId
            };

            _dbContext.Urls.Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            var result = new ResponseModel
            {
                ResponseUrl = shortUrl
            };

            return result;
        }

        public async Task<Models.Url> GetShortenedUrlAsync(string shortUrl)
        {
            return await _dbContext.Urls.FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
        }

        public async Task<IEnumerable<string>> GetUserUrls(string userId)
        {
            return await _dbContext.Urls.Where(u => u.UserID == userId).Select(u => u.ShortUrl).ToListAsync();
        }

        private string GenerateShortUrl()
        {
            var newShortUrl = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var bytes = Encoding.UTF8.GetBytes(newShortUrl);
            var result = Convert.ToBase64String(bytes).Substring(0, 8);

            return result;
        }
    }
}
