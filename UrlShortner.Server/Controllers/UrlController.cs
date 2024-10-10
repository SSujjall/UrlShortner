using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Data.Interface.IServices;
using UrlShortner.Data.Models;

namespace UrlShortner.Server.Controllers
{
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost("api/[controller]/Shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                return BadRequest("Invalid URL");
            }

            var shortenedUrl = await _urlService.CreateShortenedUrlAsync(originalUrl);
            return Ok(shortenedUrl);
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> GetOriginalUrl(string shortUrl)
        {
            var shortenedUrl = await _urlService.GetShortenedUrlAsync(shortUrl);
            if (shortenedUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortenedUrl.OriginalUrl);
        }
    }
}