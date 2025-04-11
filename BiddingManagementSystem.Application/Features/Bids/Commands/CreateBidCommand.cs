using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class CreateBidCommand : IRequest<BidDto>
    {
        public CreateBidDto BidDto { get; set; }
    }
    
    public class CreateBidCommandValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidCommandValidator()
        {
            RuleFor(x => x.BidDto)
                .NotNull().WithMessage("Bid data is required");
                
            When(x => x.BidDto != null, () => {
                RuleFor(x => x.BidDto.TenderId)
                    .NotEmpty().WithMessage("Tender ID is required");
                    
                RuleFor(x => x.BidDto.BidAmount)
                    .NotNull().WithMessage("Bid amount is required");
                    
                RuleFor(x => x.BidDto.BidAmount.Amount)
                    .GreaterThan(0).WithMessage("Bid amount must be greater than zero")
                    .When(x => x.BidDto.BidAmount != null);
                    
                RuleFor(x => x.BidDto.BidAmount.Currency)
                    .NotEmpty().WithMessage("Currency is required")
                    .MaximumLength(3).WithMessage("Currency code cannot exceed 3 characters")
                    .Matches("^[A-Z]{3}$").WithMessage("Currency must be a 3-letter code (e.g., USD, EUR)")
                    .When(x => x.BidDto.BidAmount != null);
                    
                RuleFor(x => x.BidDto.TechnicalProposalSummary)
                    .NotEmpty().WithMessage("Technical proposal summary is required")
                    .MaximumLength(2000).WithMessage("Technical proposal summary cannot exceed 2000 characters");
                    
                RuleFor(x => x.BidDto.BidItems)
                    .NotEmpty().WithMessage("At least one bid item is required");
                    
                RuleForEach(x => x.BidDto.BidItems)
                    .ChildRules(bidItem => {
                        bidItem.RuleFor(x => x.Description)
                            .NotEmpty().WithMessage("Bid item description is required")
                            .MaximumLength(500).WithMessage("Bid item description cannot exceed 500 characters");
                            
                        bidItem.RuleFor(x => x.Quantity)
                            .GreaterThan(0).WithMessage("Quantity must be greater than zero");
                            
                        bidItem.RuleFor(x => x.UnitPrice)
                            .NotNull().WithMessage("Unit price is required");
                            
                        bidItem.RuleFor(x => x.UnitPrice.Amount)
                            .GreaterThan(0).WithMessage("Unit price must be greater than zero")
                            .When(x => x.UnitPrice != null);
                            
                        bidItem.RuleFor(x => x.UnitPrice.Currency)
                            .NotEmpty().WithMessage("Currency is required")
                            .MaximumLength(3).WithMessage("Currency code cannot exceed 3 characters")
                            .Matches("^[A-Z]{3}$").WithMessage("Currency must be a 3-letter code (e.g., USD, EUR)")
                            .When(x => x.UnitPrice != null);
                            
                        bidItem.RuleFor(x => x.TotalPrice)
                            .NotNull().WithMessage("Total price is required");
                            
                        bidItem.RuleFor(x => x.TotalPrice.Amount)
                            .GreaterThan(0).WithMessage("Total price must be greater than zero")
                            .When(x => x.TotalPrice != null);
                            
                        bidItem.RuleFor(x => x.TotalPrice.Currency)
                            .NotEmpty().WithMessage("Currency is required")
                            .MaximumLength(3).WithMessage("Currency code cannot exceed 3 characters")
                            .Matches("^[A-Z]{3}$").WithMessage("Currency must be a 3-letter code (e.g., USD, EUR)")
                            .When(x => x.TotalPrice != null);
                    });
                    
                // Validate that currency is consistent across all items
                RuleFor(x => x)
                    .Must(cmd => {
                        if (cmd.BidDto.BidAmount == null || cmd.BidDto.BidItems == null || !cmd.BidDto.BidItems.Any())
                            return true;
                            
                        var currency = cmd.BidDto.BidAmount.Currency;
                        return cmd.BidDto.BidItems.All(item => 
                            item.UnitPrice?.Currency == currency && 
                            item.TotalPrice?.Currency == currency);
                    })
                    .WithMessage("All currencies must match the bid amount currency");
                    
                // Validate that total prices are calculated correctly
                RuleFor(x => x)
                    .Must(cmd => {
                        if (cmd.BidDto.BidItems == null || !cmd.BidDto.BidItems.Any())
                            return true;
                            
                        return cmd.BidDto.BidItems.All(item => 
                            item.UnitPrice != null && 
                            item.TotalPrice != null && 
                            Math.Abs(item.UnitPrice.Amount * item.Quantity - item.TotalPrice.Amount) < 0.01m);
                    })
                    .WithMessage("Total price must equal unit price multiplied by quantity");
            });
        }
    }

    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand, BidDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBidCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public CreateBidCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreateBidCommandHandler> logger,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<BidDto> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating bid for tender ID: {TenderId}", request.BidDto.TenderId);

            // Validate the tender exists
            var tenderExists = await _unitOfWork.Tenders.ExistsAsync(request.BidDto.TenderId);
            if (!tenderExists)
            {
                _logger.LogWarning("Tender with ID {TenderId} not found", request.BidDto.TenderId);
                throw new KeyNotFoundException($"Tender with ID {request.BidDto.TenderId} was not found");
            }

            // Get current user ID
            if (!_currentUserService.IsAuthenticated)
            {
                _logger.LogWarning("User not authenticated");
                throw new UnauthorizedAccessException("User must be authenticated to create a bid");
            }

            var userIdString = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userIdString))
            {
                _logger.LogWarning("User ID not found in claims");
                throw new UnauthorizedAccessException("User ID not found");
            }

            var userId = Guid.Parse(userIdString);

            // Check if user already has a bid for this tender
            var existingBid = await _unitOfWork.Bids.ExistsByTenderAndBidderAsync(request.BidDto.TenderId, userId);
            if (existingBid)
            {
                _logger.LogWarning("User {UserId} already has a bid for tender {TenderId}", userId, request.BidDto.TenderId);
                throw new InvalidOperationException($"You already have a bid for this tender. Please update your existing bid instead.");
            }

            try
            {
                // Create bid domain entity
                var bid = new Bid(
                    request.BidDto.TenderId,
                    userId,
                    new Money(request.BidDto.BidAmount.Amount, request.BidDto.BidAmount.Currency),
                    request.BidDto.TechnicalProposalSummary
                );

                // Fix for the database constraint: initialize EvaluationComments
                bid.InitializeEvaluation("");

                // Set audit fields for the bid
                bid.SetCreatedBy(userIdString);
                bid.SetUpdatedBy(userIdString);

                // Add bid items
                if (request.BidDto.BidItems != null && request.BidDto.BidItems.Any())
                {
                    foreach (var item in request.BidDto.BidItems)
                    {
                        // Add bid item and set its audit fields
                        var bidItem = bid.AddBidItem(
                            item.Description,
                            item.Quantity,
                            new Money(item.UnitPrice.Amount, item.UnitPrice.Currency)
                        );
                        
                        // Fix for the database constraint: set UpdatedBy for each BidItem
                        bidItem.SetCreatedBy(userIdString);
                        bidItem.SetUpdatedBy(userIdString);
                    }
                }
                else
                {
                    _logger.LogWarning("Bid items are missing for tender {TenderId}", request.BidDto.TenderId);
                    throw new ArgumentException("Bid must include at least one bid item");
                }

                // Save the bid
                await _unitOfWork.Bids.AddAsync(bid);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created bid ID: {BidId} for tender ID: {TenderId}", 
                    bid.Id, request.BidDto.TenderId);

                // Return DTO
                return _mapper.Map<BidDto>(bid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bid for tender ID: {TenderId}", request.BidDto.TenderId);
                throw;
            }
        }
    }
}

