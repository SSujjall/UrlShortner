using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UrlShortner.Data.Models.Config;
using UrlShortner.Data.Models.Requests;
using UrlShortner.Data.Services.ApiKey;

namespace UrlShortner.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _service;
        private readonly string _secretKey;
        public ApiKeyController(IApiKeyService service, IOptions<ApiSettings> apiSettings)
        {
            _service = service;
            _secretKey = apiSettings.Value.SecretKey;
        }

        [HttpPost("generate-new-api-key")]
        public async Task<IActionResult> GenerateApiKey([FromBody] ApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.SecretKey))
            {
                return BadRequest("Email and secret key are required.");
            }

            if (request.SecretKey != _secretKey) // Validate secret key
            {
                return Unauthorized("Invalid secret key.");
            }

            var response = await _service.GenerateApiKeyAsync(request.Email);
            return Ok(new
            {
                ApiKey = response
            });
        }

        [HttpPost("get-api-key")]
        public async Task<IActionResult> GetApiKey([FromBody] ApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.SecretKey))
            {
                return BadRequest("Email and secret key are required.");
            }

            if (request.SecretKey != _secretKey) // Validate secret key
            {
                return Unauthorized("Invalid secret key.");
            }


            var response = await _service.GetUserApiKey(request.Email);
            return Ok(new
            {
                ApiKey = response
            });
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] ApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.SecretKey))
            {
                return BadRequest("Email and secret key are required.");
            }

            if (request.SecretKey != _secretKey) // Validate secret key
            {
                return Unauthorized("Invalid secret key.");
            }


            var response = await _service.RevokeApiKeyAsync(request.Email);
            return Ok(response);
        }
    }
}
