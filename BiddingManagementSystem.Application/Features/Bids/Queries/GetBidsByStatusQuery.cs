using BiddingManagementSystem.Application.DTOs.Bids;
using BiddingManagementSystem.Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetBidsByStatusQuery : IRequest<List<BidDto>>
    {
        public BidStatus Status { get; set; }
    }
    
    public class GetBidsByStatusQueryHandler : IRequestHandler<GetBidsByStatusQuery, List<BidDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBidsByStatusQueryHandler> _logger;

        public GetBidsByStatusQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetBidsByStatusQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BidDto>> Handle(GetBidsByStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving bids with status: {Status}", request.Status);
            
            var bids = await _unitOfWork.Bids.GetByStatusAsync(request.Status);
            var bidDtos = _mapper.Map<List<BidDto>>(bids);
            
            _logger.LogInformation("Retrieved {Count} bids with status: {Status}", bidDtos.Count, request.Status);
            
            return bidDtos;
        }
    }
}