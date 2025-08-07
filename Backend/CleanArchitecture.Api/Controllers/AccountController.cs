using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            return Ok(await _accountService.LoginAsync(request, GenerateIpAddress()));
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin.ToString()));
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"].ToString());
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await _accountService.ResetPassword(model));
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {

            return Ok(await _accountService.ChangePassword(model));
        }

        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromForm] string provider, [FromForm] string idToken)
        {
            return Ok(await _accountService.ExternalLoginAsync(provider, idToken, GenerateIpAddress()));
        }

        [HttpPost("external-register")]
        public async Task<IActionResult> ExternalRegister([FromForm] string provider, [FromForm] string idToken)
        {
            return Ok(await _accountService.ExternalRegisterAsync(provider, idToken, GenerateIpAddress()));
        }

        [NonAction]
        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"].ToString();
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;
        }
    }
}
