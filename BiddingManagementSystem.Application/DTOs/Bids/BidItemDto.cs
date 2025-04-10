using System;

namespace BiddingManagementSystem.Application.DTOs.Bids
{
    public class BidItemDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public MoneyDto UnitPrice { get; set; }
        public MoneyDto TotalPrice { get; set; }
    }
}