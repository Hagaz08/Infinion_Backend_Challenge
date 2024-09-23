using Infinion.Core.Abstractions;
using Infinion.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Infinion_Sadiq.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterUserDto requestDTO)
        {
            
            var response = await _authService.Register(requestDTO);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("admin_register")]

        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto requestDTO)
        {
            var response = await _authService.RegisterAdmin(requestDTO);

            if (response.IsSuccess)
                return Ok(response);

          return BadRequest(response);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginUserDto requestDTO)
        {
            var response = await _authService.Login(requestDTO);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        

        [HttpPost("forgot_password")]

        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var response = await _authService.ForgotPasswordAsync(forgotPasswordDto);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("reset_password")]

        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromQuery] string token, [FromQuery] string email)
        {
            var response = await _authService.ResetPasswordAsync(email,token,resetPasswordDto);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("confirm_email")]

        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            var response = await _authService.ConfirmEmailAsymc(token, email);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
