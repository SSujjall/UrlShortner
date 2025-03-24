using Microsoft.EntityFrameworkCore;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;
using UrlShortner.Data.Repositories.ApiKey;

namespace UrlShortner.Server.Middlewares
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiKeyRepository apiKeyRepo)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key Missing.");
                return;
            }

            var keyRecord = await apiKeyRepo.CheckApiKey(extractedApiKey);

            if (keyRecord == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid or revoked API Key.");
                return;
            }
            context.Items["UserId"] = keyRecord.UserId;
            await _next(context);
        }
    }
}
