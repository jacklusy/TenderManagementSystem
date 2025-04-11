using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class AddTenderDocumentCommand : IRequest<TenderDocumentDto>
    {
        public Guid TenderId { get; set; }
        public UploadTenderDocumentDto DocumentDto { get; set; }
    }
} 