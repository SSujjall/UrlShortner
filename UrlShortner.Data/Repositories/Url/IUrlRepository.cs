using UrlShortner.Data.Models;

namespace UrlShortner.Data.Repositories.Url
{
    public interface IUrlRepository
    {
        Task<Models.Url> GetShortenedUrlAsync(string shortUrl);
        Task<ResponseModel> CreateShortenedUrlAsync(string userId, string originalUrl);
        Task<IEnumerable<string>> GetUserUrls(string userId);
    }
}
