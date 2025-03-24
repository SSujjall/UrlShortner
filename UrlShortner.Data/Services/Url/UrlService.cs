using UrlShortner.Data.Models;
using UrlShortner.Data.Repositories.Url;

namespace UrlShortner.Data.Services.Url
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<ResponseModel> CreateShortenedUrlAsync(string userId, string originalUrl)
        {
            return await _urlRepository.CreateShortenedUrlAsync(userId, originalUrl);
        }

        public async Task<Models.Url> GetShortenedUrlAsync(string shortUrl)
        {
            return await _urlRepository.GetShortenedUrlAsync(shortUrl);
        }

        public async Task<IEnumerable<string>> GetUserUrls(string userId)
        {
            return await _urlRepository.GetUserUrls(userId);
        }
    }
}
