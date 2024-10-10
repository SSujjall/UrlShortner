using Microsoft.AspNetCore.Mvc;

namespace UrlShortner.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly string baseUrl = "https://localhost:7244";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> CreateShortUrl(string url)
        {
            return null;
        }
    }
}
