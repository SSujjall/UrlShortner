using UrlShortner.Data.Models;

namespace UrlShortner.Data.Services.Url
{
    public interface IUrlService
    {
        Task<Models.Url> GetShortenedUrlAsync(string shortUrl);
        Task<ResponseModel> CreateShortenedUrlAsync(string userId, string originalUrl);
        Task<IEnumerable<string>> GetUserUrls(string userId);
    }
}
