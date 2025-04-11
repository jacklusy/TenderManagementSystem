using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetBidByIdQuery : IRequest<BidDto>
    {
        public Guid Id { get; set; }
    }
    
    public class GetBidByIdQueryHandler : IRequestHandler<GetBidByIdQuery, BidDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBidByIdQueryHandler> _logger;

        public GetBidByIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetBidByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BidDto> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving bid with ID: {BidId}", request.Id);
            
            var bid = await _unitOfWork.Bids.GetByIdAsync(request.Id);
            
            if (bid == null)
            {
                _logger.LogWarning("Bid with ID {BidId} not found", request.Id);
                throw new KeyNotFoundException($"Bid with ID {request.Id} was not found");
            }
            
            var bidDto = _mapper.Map<BidDto>(bid);
            
            _logger.LogInformation("Successfully retrieved bid with ID: {BidId}", request.Id);
            
            return bidDto;
        }
    }
}