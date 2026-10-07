using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using UrlShortner.Client.Services;

namespace UrlShortner.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;

        public HomeController(ApiClient api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            if (_api.CurrentKey == null) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl(string url)
        {
            var key = _api.CurrentKey;
            if (key == null) return RedirectToAction("Login", "Account");

            var response = await _api.Send(HttpMethod.Post, "/api/Url/Shorten", key, new { OriginalUrl = url });

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<UrlShortner.Client.Models.ResponseModel>(data,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (model != null)
                    ViewBag.ShortenedUrl = $"{_api.BaseUrl}/{model.responseUrl}";
                return View("Index");
            }

            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                return ExpireSession();

            ModelState.AddModelError("", "Error creating shortened URL");
            return View("Index");
        }

        public async Task<IActionResult> History()
        {
            var key = _api.CurrentKey;
            if (key == null) return RedirectToAction("Login", "Account");

            var response = await _api.Send(HttpMethod.Get, "/history", key);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                return ExpireSession();

            var codes = response.IsSuccessStatusCode
                ? JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync()) ?? new()
                : new List<string>();
            if (!response.IsSuccessStatusCode)
                ViewBag.Error = "Could not load history.";

            ViewBag.BaseUrl = _api.BaseUrl;
            return View(codes);
        }

        private IActionResult ExpireSession()
        {
            Response.Cookies.Delete(ApiClient.KeyCookie);
            TempData["Error"] = "Your API key is invalid or revoked. Please log in again.";
            return RedirectToAction("Login", "Account");
        }
    }
}
