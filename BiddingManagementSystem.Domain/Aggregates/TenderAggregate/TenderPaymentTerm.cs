using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.TenderAggregate
{
    public class TenderPaymentTerm : BaseEntity
    {
        public string Description { get; private set; }
        public decimal Percentage { get; private set; }
        
        private TenderPaymentTerm() { }
        
        public TenderPaymentTerm(string description, decimal percentage)
        {
            Description = description;
            Percentage = percentage;
        }
    }
}