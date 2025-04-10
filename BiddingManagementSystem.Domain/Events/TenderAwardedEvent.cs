using System;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;

namespace BiddingManagementSystem.Domain.Events
{
    public class TenderAwardedEvent
    {
        public Guid TenderId { get; }
        public string TenderTitle { get; }
        public Guid WinningBidId { get; }
        
        public TenderAwardedEvent(Tender tender)
        {
            TenderId = tender.Id;
            TenderTitle = tender.Title;
            WinningBidId = tender.WinningBidId.Value;
        }
    }
}