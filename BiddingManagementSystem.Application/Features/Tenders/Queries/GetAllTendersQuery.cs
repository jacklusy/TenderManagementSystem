using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetAllTendersQuery : IRequest<List<TenderDto>>
    {
    }
    
    public class GetAllTendersQueryHandler : IRequestHandler<GetAllTendersQuery, List<TenderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTendersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TenderDto>> Handle(GetAllTendersQuery request, CancellationToken cancellationToken)
        {
            var tenders = await _unitOfWork.Tenders.GetAllAsync();
            return _mapper.Map<List<TenderDto>>(tenders);
        }
    }
}