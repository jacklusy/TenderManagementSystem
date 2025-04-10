using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.DTOs.Auth;
using BiddingManagementSystem.Application.Features.Users.Commands;
using BiddingManagementSystem.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiddingManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var result = await _mediator.Send(new GetUserByUsernameQuery { Username = username });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] RegisterUserDto registerUserDto)
        {
            var command = new CreateUserCommand { RegisterUserDto = registerUserDto };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            var command = new UpdateUserCommand
            {
                Id = id,
                UserDto = updateUserDto
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/activate")]
        public async Task<ActionResult<UserDto>> ActivateUser(Guid id)
        {
            var command = new ActivateUserCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult<UserDto>> DeactivateUser(Guid id)
        {
            var command = new DeactivateUserCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
} 