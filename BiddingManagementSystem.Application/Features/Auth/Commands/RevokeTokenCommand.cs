using System.Threading;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.Contracts;
using FluentValidation;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Auth.Commands
{
    public class RevokeTokenCommand : IRequest
    {
        public string Token { get; set; }
    }

    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }

    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IIdentityService _identityService;

        public RevokeTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            await _identityService.RevokeTokenAsync(request.Token);
            return Unit.Value;
        }
    }
} 