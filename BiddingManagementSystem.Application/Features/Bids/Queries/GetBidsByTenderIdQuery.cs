using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetBidsByTenderIdQuery : IRequest<List<BidDto>>
    {
        public Guid TenderId { get; set; }
    }
    
    public class GetBidsByTenderIdQueryHandler : IRequestHandler<GetBidsByTenderIdQuery, List<BidDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBidsByTenderIdQueryHandler> _logger;

        public GetBidsByTenderIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetBidsByTenderIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BidDto>> Handle(GetBidsByTenderIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving bids for tender ID: {TenderId}", request.TenderId);
            
            // First check if the tender exists
            var tenderExists = await _unitOfWork.Tenders.ExistsAsync(request.TenderId);
            if (!tenderExists)
            {
                _logger.LogWarning("Tender with ID {TenderId} not found", request.TenderId);
                throw new KeyNotFoundException($"Tender with ID {request.TenderId} was not found");
            }
            
            var bids = await _unitOfWork.Bids.GetByTenderIdAsync(request.TenderId);
            var bidDtos = _mapper.Map<List<BidDto>>(bids);
            
            _logger.LogInformation("Retrieved {Count} bids for tender ID: {TenderId}", bidDtos.Count, request.TenderId);
            
            return bidDtos;
        }
    }
}