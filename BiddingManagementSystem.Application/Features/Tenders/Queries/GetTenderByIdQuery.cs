using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.Exceptions;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTenderByIdQuery : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
    }

    public class GetTenderByIdQueryHandler : IRequestHandler<GetTenderByIdQuery, TenderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTenderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TenderDto> Handle(GetTenderByIdQuery request, CancellationToken cancellationToken)
        {
            var tender = await _unitOfWork.Tenders.GetByIdAsync(request.Id);
            
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender with ID {request.Id} was not found.");
            }
            
            return _mapper.Map<TenderDto>(tender);
        }
    }
}