using System.Threading.Tasks;
using BiddingManagementSystem.Application.DTOs.Auth;
using BiddingManagementSystem.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiddingManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserDto registerDto)
        {
            var command = new RegisterCommand { RegisterUserDto = registerDto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var command = new LoginCommand 
            { 
                Username = loginDto.Username, 
                Password = loginDto.Password 
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var command = new RefreshTokenCommand
            {
                AccessToken = refreshTokenDto.AccessToken,
                RefreshToken = refreshTokenDto.RefreshToken
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            var command = new RevokeTokenCommand { Token = request.Token };
            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var command = new ChangePasswordCommand
            {
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };
            var result = await _mediator.Send(command);
            
            if (result)
                return Ok(new { message = "Password changed successfully" });
            
            return BadRequest(new { message = "Failed to change password" });
        }
    }

    public class RevokeTokenRequest
    {
        public string Token { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
} 