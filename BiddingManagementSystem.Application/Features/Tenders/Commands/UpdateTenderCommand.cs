using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class UpdateTenderCommand : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
        public UpdateTenderDto TenderDto { get; set; }
    }
    
    public class UpdateTenderCommandHandler : IRequestHandler<UpdateTenderCommand, TenderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTenderCommandHandler> _logger;

        public UpdateTenderCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<UpdateTenderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TenderDto> Handle(UpdateTenderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating tender with ID: {TenderId}", request.Id);
            
            // Get the existing tender
            var tender = await _unitOfWork.Tenders.GetByIdAsync(request.Id);
            if (tender == null)
            {
                _logger.LogWarning("Tender with ID {TenderId} not found", request.Id);
                throw new KeyNotFoundException($"Tender with ID {request.Id} was not found");
            }
            
            // Update the tender properties
            tender.UpdateDetails(
                request.TenderDto.Title,
                request.TenderDto.Description,
                request.TenderDto.ClosingDate,
                request.TenderDto.Category,
                new Money(request.TenderDto.BudgetRange.Amount, request.TenderDto.BudgetRange.Currency),
                request.TenderDto.ContactEmail
            );
            
            // Save changes
            await _unitOfWork.Tenders.UpdateAsync(tender);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Successfully updated tender with ID: {TenderId}", request.Id);
            
            // Return the updated tender
            return _mapper.Map<TenderDto>(tender);
        }
    }
}