using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Application.DTOs.Auth;
using FluentValidation;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<AuthResponseDto>
    {
        public RegisterUserDto RegisterUserDto { get; set; }
    }

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.RegisterUserDto.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.RegisterUserDto.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.RegisterUserDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is invalid");

            RuleFor(x => x.RegisterUserDto.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.RegisterUserDto.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.RegisterUserDto.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

            RuleFor(x => x.RegisterUserDto.Role)
                .IsInEnum().WithMessage("Invalid role");

            // Conditional validation for company details when role is Bidder
            When(x => x.RegisterUserDto.Role == Domain.Enums.UserRole.Bidder, () =>
            {
                RuleFor(x => x.RegisterUserDto.CompanyName)
                    .NotEmpty().WithMessage("Company name is required for bidders")
                    .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters");

                RuleFor(x => x.RegisterUserDto.RegistrationNumber)
                    .NotEmpty().WithMessage("Registration number is required for bidders")
                    .MaximumLength(50).WithMessage("Registration number cannot exceed 50 characters");

                RuleFor(x => x.RegisterUserDto.CompanyAddress)
                    .NotNull().WithMessage("Company address is required for bidders");

                RuleFor(x => x.RegisterUserDto.CompanyAddress.Country)
                    .NotEmpty().WithMessage("Country is required")
                    .When(x => x.RegisterUserDto.CompanyAddress != null);

                RuleFor(x => x.RegisterUserDto.CompanyAddress.City)
                    .NotEmpty().WithMessage("City is required")
                    .When(x => x.RegisterUserDto.CompanyAddress != null);
            });
        }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RegisterUserAsync(request.RegisterUserDto);
        }
    }
} 