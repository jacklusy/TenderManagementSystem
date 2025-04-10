using System.Threading;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Application.DTOs.Auth;
using FluentValidation;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required");
        }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenDto = new RefreshTokenDto
            {
                AccessToken = request.AccessToken,
                RefreshToken = request.RefreshToken
            };

            return await _identityService.RefreshTokenAsync(refreshTokenDto);
        }
    }
} 