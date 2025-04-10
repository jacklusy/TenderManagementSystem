using System;

namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class TenderPaymentTermDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
    }
}