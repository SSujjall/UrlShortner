using Microsoft.AspNetCore.Mvc;
using UrlShortner.Data.Models.Requests;
using UrlShortner.Data.Services.Url;

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
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrlRequest req)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User ID not found.");
            }

            var userId = (string)HttpContext.Items["UserId"];

            if (string.IsNullOrWhiteSpace(req.OriginalUrl))
            {
                return BadRequest("Invalid URL");
            }

            var shortenedUrl = await _urlService.CreateShortenedUrlAsync(userId, req.OriginalUrl);
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

        [HttpGet("history")]
        public async Task<IActionResult> GetUrlHistory()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User ID not found.");
            }
            var userId = (string)HttpContext.Items["UserId"];

            var urls = await _urlService.GetUserUrls(userId);
            return Ok(urls);
        }
    }
}