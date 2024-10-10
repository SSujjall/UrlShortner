using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Models;

namespace UrlShortner.Data.Interface.IServices
{
    public interface IUrlService
    {
        Task<Url> GetShortenedUrlAsync(string shortUrl);
        Task<ResponseModel> CreateShortenedUrlAsync(string originalUrl);
    }
}
