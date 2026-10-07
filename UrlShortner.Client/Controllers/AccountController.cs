using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortner.Client.Services;

namespace UrlShortner.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _api;

        public AccountController(ApiClient api)
        {
            _api = api;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string apiKey)
        {
            apiKey = (apiKey ?? "").Trim();
            if (apiKey.Length == 0)
            {
                TempData["Error"] = "API key is required.";
                return View();
            }

            // history is the cheapest authenticated endpoint; use it to validate the key
            var response = await _api.Send(HttpMethod.Get, "/history", apiKey);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                TempData["Error"] = "Invalid or revoked API key.";
                return View();
            }
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not reach the server. Try again.";
                return View();
            }

            Response.Cookies.Append(ApiClient.KeyCookie, apiKey, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(ApiClient.KeyCookie);
            return RedirectToAction("Login");
        }

        public IActionResult GetKey() => View();

        [HttpPost]
        public async Task<IActionResult> GetKey(string email)
        {
            var response = await _api.PostPublic("/api/ApiKey/user-generate-new-key", new { Email = email });
            await SetResult(response, "Your API key was sent to your email.");
            return View();
        }

        public IActionResult ForgotKey() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotKey(string email)
        {
            var response = await _api.PostPublic("/api/ApiKey/forgot-key", new { Email = email });
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "An OTP was sent to your email.";
                return RedirectToAction("ChangeKey", new { email });
            }
            await SetResult(response, "");
            return View();
        }

        public IActionResult ChangeKey(string? email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeKey(string email, string otp)
        {
            var response = await _api.PostPublic("/api/ApiKey/change-key", new { Email = email, Otp = otp });
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Your new API key was sent to your email.";
                return RedirectToAction("Login");
            }
            await SetResult(response, "");
            ViewBag.Email = email;
            return View();
        }

        private async Task SetResult(HttpResponseMessage response, string success)
        {
            if (response.IsSuccessStatusCode) TempData["Success"] = success;
            else
            {
                var msg = (await response.Content.ReadAsStringAsync()).Trim('"');
                TempData["Error"] = string.IsNullOrWhiteSpace(msg) ? "Request failed." : msg;
            }
        }
    }
}
