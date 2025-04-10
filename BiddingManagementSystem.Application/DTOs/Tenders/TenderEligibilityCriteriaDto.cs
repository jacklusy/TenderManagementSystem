using System;

namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class TenderEligibilityCriteriaDto
    {
        public Guid Id { get; set; }
        public string CriteriaName { get; set; }
        public string CriteriaDescription { get; set; }
    }
}