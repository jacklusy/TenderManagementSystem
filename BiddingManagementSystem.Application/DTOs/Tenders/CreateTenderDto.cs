using System;
using System.Collections.Generic;
using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Application.DTOs.Tenders
{
    public class CreateTenderDto
    {
        public string Title { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public TenderType Type { get; set; }
        public string Category { get; set; }
        public MoneyDto BudgetRange { get; set; }
        public string ContactEmail { get; set; }
        public string IssuedByOrganization { get; set; }
        
        public List<TenderEligibilityCriteriaDto> EligibilityCriteria { get; set; }
        public List<TenderDeliverableDto> Deliverables { get; set; }
        public List<TenderActivityDto> Timeline { get; set; }
        public List<TenderPaymentTermDto> PaymentTerms { get; set; }
    }
}