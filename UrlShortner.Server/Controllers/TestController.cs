using Microsoft.AspNetCore.Mvc;
using UrlShortner.Data.Services.Email;
using UrlShortner.Data.Services.Email.EmailModel;

namespace UrlShortner.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-mail-test")]
        public async Task<IActionResult> SendMailTest([FromBody] EmailMessage request)
        {
            try
            {
                await _emailService.SendMail(request);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
