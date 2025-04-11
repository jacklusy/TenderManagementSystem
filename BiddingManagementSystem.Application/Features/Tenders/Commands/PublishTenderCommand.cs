using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class PublishTenderCommand : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
    }
    
    public class PublishTenderCommandHandler : IRequestHandler<PublishTenderCommand, TenderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PublishTenderCommandHandler> _logger;

        public PublishTenderCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<PublishTenderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TenderDto> Handle(PublishTenderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing tender with ID: {TenderId}", request.Id);
            
            // Get the tender
            var tender = await _unitOfWork.Tenders.GetByIdAsync(request.Id);
            if (tender == null)
            {
                _logger.LogWarning("Tender with ID {TenderId} not found", request.Id);
                throw new KeyNotFoundException($"Tender with ID {request.Id} was not found");
            }
            
            try
            {
                // Publish the tender
                tender.Publish();
                
                // Save changes
                await _unitOfWork.Tenders.UpdateAsync(tender);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Successfully published tender with ID: {TenderId}", request.Id);
                
                // Return the updated tender
                return _mapper.Map<TenderDto>(tender);
            }
            catch (InvalidTenderOperationException ex)
            {
                _logger.LogWarning("Failed to publish tender with ID {TenderId}: {Message}", request.Id, ex.Message);
                throw;
            }
        }
    }
} 