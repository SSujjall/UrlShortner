using Microsoft.AspNetCore.Mvc;
using UrlShortner.Data.Models.Requests;
using UrlShortner.Data.Services.ApiKey;

namespace UrlShortner.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _service;
        public ApiKeyController(IApiKeyService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateApiKey([FromBody] ApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            var response = await _service.GenerateApiKeyAsync(request.Email);
            return Ok(new
            {
                ApiKey = response
            });
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] ApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            await _service.RevokeApiKeyAsync(request.Email);
            return Ok("API Key Revoked");
        }
    }
}
