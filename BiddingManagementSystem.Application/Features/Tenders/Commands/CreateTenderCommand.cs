using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Application.DTOs.Tenders;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using BiddingManagementSystem.Domain.Exceptions;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class CreateTenderCommand : IRequest<TenderDto>
    {
        public CreateTenderDto TenderDto { get; set; }
    }

    public class CreateTenderCommandHandler : IRequestHandler<CreateTenderCommand, TenderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTenderCommandHandler> _logger;

        public CreateTenderCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<CreateTenderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TenderDto> Handle(CreateTenderCommand request, CancellationToken cancellationToken)
        {
            // Check if tender with same reference number already exists
            var referenceNumber = request.TenderDto.ReferenceNumber;
            
            _logger.LogInformation("Checking if tender with reference number {ReferenceNumber} already exists", referenceNumber);
            
            if (await _unitOfWork.Tenders.ExistsByReferenceNumberAsync(referenceNumber))
            {
                _logger.LogWarning("Attempt to create tender with duplicate reference number: {ReferenceNumber}", referenceNumber);
                throw new DuplicateTenderReferenceException(referenceNumber);
            }
            
            // Map DTO to domain entity
            var tender = _mapper.Map<Tender>(request.TenderDto);
            
            // Set initial status which includes setting the UpdatedBy field
            tender.SetInitialStatus();
            
            // Explicitly set UpdatedBy to the same value as CreatedBy to avoid NULL
            // This is needed because the database doesn't allow NULL for UpdatedBy
            tender.SetUpdatedBy("System");
            
            // Save to database
            await _unitOfWork.Tenders.AddAsync(tender);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Successfully created tender with ID: {TenderId}, Reference: {ReferenceNumber}", 
                tender.Id, referenceNumber);
            
            // Return mapped DTO
            return _mapper.Map<TenderDto>(tender);
        }
    }
}
