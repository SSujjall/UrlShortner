using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Ocsp;
using UrlShortner.Data.Models.Config;
using UrlShortner.Data.Models.Requests;
using UrlShortner.Data.Services.ApiKey;
using UrlShortner.Data.Services.Email;
using UrlShortner.Data.Services.Email.EmailModel;
using UrlShortner.Data.Services.ForgotKey;

namespace UrlShortner.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _service;
        private readonly string _secretKey;
        private readonly IEmailService _emailService;
        private readonly IForgotKeyService _fpService;

        public ApiKeyController(IApiKeyService service, IOptions<ApiSettings> apiSettings,
            IEmailService emailService, IForgotKeyService fpService)
        {
            _service = service;
            _secretKey = apiSettings.Value.SecretKey;
            _emailService = emailService;
            _fpService = fpService;
        }

        [HttpPost("user-generate-new-key")]
        public async Task<IActionResult> GenerateNewKey([FromBody] UserApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            var response = await _service.GenerateApiKeyAsync(request.Email);
            if (string.IsNullOrEmpty(response))
            {
                return BadRequest("Failed to generate API key.");
            }

            if (response.Contains("Cannot create new api key"))
            {
                return BadRequest(response);
            }

            var msg = new EmailMessage
            {
                To = request.Email,
                Subject = "UrlShortner Login Key",
                Content = $"Your login key is: {response}"
            };

            try
            {
                await _emailService.SendMail(msg);
                return Ok("Api key sent to email.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email. Error: {ex.Message}");
            }
        }

        [HttpPost("forgot-key")]
        public async Task<IActionResult> ForgotKey([FromBody] UserApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            var result = await _fpService.RequestOtp(request.Email);
            if (result != null)
            {
                var msg = new EmailMessage
                {
                    To = request.Email,
                    Subject = "UrlShortner Forgot Key",
                    Content = $"Your OTP is: {result}"
                };
                try
                {
                    await _emailService.SendMail(msg);
                    return Ok("Otp sent to email.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to send email. Error: {ex.Message}");
                }
            }
            else
            {
                return BadRequest("Failed to send otp.");
            }
        }

        [HttpPost("change-key")]
        public async Task<IActionResult> ChangeKey([FromBody] KeyChangeOtpRequest req)
        {
            if (string.IsNullOrEmpty(req.Otp))
            {
                return BadRequest("otp is required.");
            }

            var response = await _fpService.VerifyOtpAndResetKey(req.Email, req.Otp);
            if (string.IsNullOrEmpty(response))
            {
                return BadRequest("Failed to generate API key.");
            }

            var msg = new EmailMessage
            {
                To = req.Email,
                Subject = "UrlShortner Login Key",
                Content = $"Your new login key is: {response}"
            };

            try
            {
                await _emailService.SendMail(msg);
                return Ok("Api key sent to email.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email. Error: {ex.Message}");
            }
        }

        #region admin methods
        [HttpPost("generate-new-api-key-admin")]
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
        #endregion
    }
}
