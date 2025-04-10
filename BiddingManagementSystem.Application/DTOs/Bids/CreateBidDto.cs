using System;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.DTOs.Bids
{
    public class CreateBidDto
    {
        public Guid TenderId { get; set; }
        public MoneyDto BidAmount { get; set; }
        public string TechnicalProposalSummary { get; set; }
        public List<BidItemDto> BidItems { get; set; }
    }
}