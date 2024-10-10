using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Interface.IRepositories;
using UrlShortner.Data.Interface.IServices;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Implementation.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<ResponseModel> CreateShortenedUrlAsync(string originalUrl)
        {
            return await _urlRepository.CreateShortenedUrlAsync(originalUrl);
        }

        public async Task<Url> GetShortenedUrlAsync(string shortUrl)
        {
            return await _urlRepository.GetShortenedUrlAsync(shortUrl);
        }
    }
}
