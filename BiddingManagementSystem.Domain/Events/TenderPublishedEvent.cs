using System;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;

namespace BiddingManagementSystem.Domain.Events
{
    public class TenderPublishedEvent
    {
        public Guid TenderId { get; }
        public string TenderTitle { get; }
        public string ReferenceNumber { get; }
        public DateTime ClosingDate { get; }
        
        public TenderPublishedEvent(Tender tender)
        {
            TenderId = tender.Id;
            TenderTitle = tender.Title;
            ReferenceNumber = tender.ReferenceNumber;
            ClosingDate = tender.ClosingDate;
        }
    }
}