using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;
using BiddingManagementSystem.Application.Contracts;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetCurrentUserBidsQuery : IRequest<List<BidDto>>
    {
    }
    
    public class GetCurrentUserBidsQueryHandler : IRequestHandler<GetCurrentUserBidsQuery, List<BidDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCurrentUserBidsQueryHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserBidsQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetCurrentUserBidsQueryHandler> logger,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<List<BidDto>> Handle(GetCurrentUserBidsQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
            {
                _logger.LogWarning("Attempt to get current user bids without authentication");
                throw new UnauthorizedAccessException("User must be authenticated to access their bids");
            }
            
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID not found in claims");
                throw new UnauthorizedAccessException("User ID not found");
            }
            
            _logger.LogInformation("Retrieving bids for current user with ID: {UserId}", userId);
            
            var bids = await _unitOfWork.Bids.GetByBidderIdAsync(Guid.Parse(userId));
            var bidDtos = _mapper.Map<List<BidDto>>(bids);
            
            _logger.LogInformation("Retrieved {Count} bids for user ID: {UserId}", bidDtos.Count, userId);
            
            return bidDtos;
        }
    }
}