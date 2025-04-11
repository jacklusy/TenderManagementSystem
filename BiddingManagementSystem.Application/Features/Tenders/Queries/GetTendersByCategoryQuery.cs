using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTendersByCategoryQuery : IRequest<List<TenderDto>>
    {
        public string Category { get; set; }
    }
    
    public class GetTendersByCategoryQueryHandler : IRequestHandler<GetTendersByCategoryQuery, List<TenderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTendersByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TenderDto>> Handle(GetTendersByCategoryQuery request, CancellationToken cancellationToken)
        {
            var tenders = await _unitOfWork.Tenders.GetByCategoryAsync(request.Category);
            return _mapper.Map<List<TenderDto>>(tenders);
        }
    }
}