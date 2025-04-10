using System.Threading;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Application.DTOs.Auth;
using FluentValidation;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = new LoginDto
            {
                Username = request.Username,
                Password = request.Password
            };

            return await _identityService.LoginAsync(loginDto);
        }
    }
} 