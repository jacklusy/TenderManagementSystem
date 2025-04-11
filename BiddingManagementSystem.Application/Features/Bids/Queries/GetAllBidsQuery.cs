using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetAllBidsQuery : IRequest<List<BidDto>>
    {
    }
    
    public class GetAllBidsQueryHandler : IRequestHandler<GetAllBidsQuery, List<BidDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllBidsQueryHandler> _logger;

        public GetAllBidsQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<GetAllBidsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BidDto>> Handle(GetAllBidsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all bids");
            
            var bids = await _unitOfWork.Bids.GetAllAsync();
            
            var bidDtos = _mapper.Map<List<BidDto>>(bids);
            
            _logger.LogInformation("Retrieved {Count} bids", bidDtos.Count);
            
            return bidDtos;
        }
    }
}

