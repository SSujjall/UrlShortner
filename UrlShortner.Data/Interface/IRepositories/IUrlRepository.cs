using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Models;

namespace UrlShortner.Data.Interface.IRepositories
{
    public interface IUrlRepository
    {
        Task<Url> GetShortenedUrlAsync(string shortUrl);
        Task<Url> CreateShortenedUrlAsync(string originalUrl);
    }
}
