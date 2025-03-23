using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UrlShortner.Client.Models;
using UrlShortner.Data.Models;

namespace UrlShortner.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly string baseUrl = "https://localhost:7244";
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl(string url)
        {
            var apiUrl = $"{baseUrl}/api/Url/Shorten";

            var json = JsonSerializer.Serialize(url);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var shortenedUrl = JsonSerializer.Deserialize<UrlShortner.Client.Models.ResponseModel>(responseData);
                if (shortenedUrl != null)
                {
                    ViewBag.ShortenedUrl = $"{baseUrl}/{shortenedUrl?.responseUrl}";
                }
                return View("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error creating shortened URL");
                return View("Index");
            }
        }
    }
}
