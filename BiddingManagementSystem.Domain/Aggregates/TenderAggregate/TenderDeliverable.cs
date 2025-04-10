using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.TenderAggregate
{
    public class TenderDeliverable : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        
        private TenderDeliverable() { }
        
        public TenderDeliverable(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}