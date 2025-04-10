using System;
using System.Collections.Generic;
using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Application.DTOs.Bids
{
    public class BidDto
    {
        public Guid Id { get; set; }
        public Guid TenderId { get; set; }
        public Guid BidderId { get; set; }
        public BidStatus Status { get; set; }
        public MoneyDto BidAmount { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string TechnicalProposalSummary { get; set; }
        public decimal? Score { get; set; }
        public string EvaluationComments { get; set; }
        
        public List<BidItemDto> BidItems { get; set; }
        public List<BidDocumentDto> Documents { get; set; }
    }
}