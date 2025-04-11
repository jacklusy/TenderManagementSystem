using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class RemoveTenderDocumentCommand : IRequest<Unit>
    {
        public Guid TenderId { get; set; }
        public Guid DocumentId { get; set; }
    }
} 