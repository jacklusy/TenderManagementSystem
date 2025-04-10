using System;
using BiddingManagementSystem.Domain.Common;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Domain.Aggregates.BidAggregate
{
    public class BidItem : BaseEntity
    {
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money TotalPrice { get; private set; }
        
        private BidItem() { }
        
        public BidItem(string description, int quantity, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty", nameof(description));
                
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
                
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
            
            // Calculate total price
            TotalPrice = new Money(unitPrice.Amount * quantity, unitPrice.Currency);
        }
    }
}