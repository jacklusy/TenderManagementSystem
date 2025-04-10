using System;

namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class TenderActivityDto
    {
        public Guid Id { get; set; }
        public string ActivityName { get; set; }
        public DateTime ExpectedDate { get; set; }
    }
}