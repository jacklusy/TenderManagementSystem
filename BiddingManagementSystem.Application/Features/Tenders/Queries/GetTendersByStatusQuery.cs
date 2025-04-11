using BiddingManagementSystem.Application.DTOs.Tenders;
using BiddingManagementSystem.Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTendersByStatusQuery : IRequest<List<TenderDto>>
    {
        public TenderStatus Status { get; set; }
    }
    
    public class GetTendersByStatusQueryHandler : IRequestHandler<GetTendersByStatusQuery, List<TenderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTendersByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TenderDto>> Handle(GetTendersByStatusQuery request, CancellationToken cancellationToken)
        {
            var tenders = await _unitOfWork.Tenders.GetByStatusAsync(request.Status);
            return _mapper.Map<List<TenderDto>>(tenders);
        }
    }
}