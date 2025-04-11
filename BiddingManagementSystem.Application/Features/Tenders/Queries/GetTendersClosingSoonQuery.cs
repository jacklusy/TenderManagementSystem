using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTendersClosingSoonQuery : IRequest<List<TenderDto>>
    {
        public int DaysThreshold { get; set; }
    }
    
    public class GetTendersClosingSoonQueryHandler : IRequestHandler<GetTendersClosingSoonQuery, List<TenderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTendersClosingSoonQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TenderDto>> Handle(GetTendersClosingSoonQuery request, CancellationToken cancellationToken)
        {
            var tenders = await _unitOfWork.Tenders.GetClosingSoonAsync(request.DaysThreshold);
            return _mapper.Map<List<TenderDto>>(tenders);
        }
    }
} 