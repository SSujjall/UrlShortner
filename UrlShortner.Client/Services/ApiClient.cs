using System.Net.Http.Json;
using UrlShortner.Data.Models;

namespace UrlShortner.Client.Services
{
    public class ApiClient
    {
        public const string KeyCookie = "api_key";

        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;

        public string BaseUrl { get; }

        public ApiClient(HttpClient http, IHttpContextAccessor ctx, IConfiguration config)
        {
            _http = http;
            _ctx = ctx;
            BaseUrl = config["ApiBaseUrl"]!;
        }

        public string? CurrentKey => _ctx.HttpContext?.Request.Cookies[KeyCookie];

        public Task<HttpResponseMessage> PostPublic(string path, object body) =>
            _http.PostAsJsonAsync($"{BaseUrl}{path}", body);

        public Task<HttpResponseMessage> Send(HttpMethod method, string path, string key, object? body = null)
        {
            var req = new HttpRequestMessage(method, $"{BaseUrl}{path}");
            req.Headers.Add(AuthConstants.ApiKeyHeaderName, key);
            if (body != null) req.Content = JsonContent.Create(body);
            return _http.SendAsync(req);
        }
    }
}
