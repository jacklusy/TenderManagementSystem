using System;
using BiddingManagementSystem.Domain.Aggregates.BidAggregate;

namespace BiddingManagementSystem.Domain.Events
{
    public class BidSubmittedEvent
    {
        public Guid BidId { get; }
        public Guid TenderId { get; }
        public Guid BidderId { get; }
        public DateTime SubmissionDate { get; }
        
        public BidSubmittedEvent(Bid bid)
        {
            BidId = bid.Id;
            TenderId = bid.TenderId;
            BidderId = bid.BidderId;
            SubmissionDate = bid.SubmissionDate;
        }
    }
}