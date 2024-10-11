using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Interface.IRepositories;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Implementation.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _dbContext;

        public UrlRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseModel> CreateShortenedUrlAsync(string originalUrl)
        {
            var shortUrl = GenerateShortUrl();
            var shortenedUrl = new Url
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl
            };

            _dbContext.Urls.Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            var result = new ResponseModel
            {
                ResponseUrl = shortUrl
            };

            return result;
        }

        public async Task<Url> GetShortenedUrlAsync(string shortUrl)
        {
            return await _dbContext.Urls.FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
        }

        private string GenerateShortUrl()
        {
            var newShortUrl = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var bytes = System.Text.Encoding.UTF8.GetBytes(newShortUrl);
            var result = System.Convert.ToBase64String(bytes).Substring(0, 8);

            return result;
        }
    }
}
