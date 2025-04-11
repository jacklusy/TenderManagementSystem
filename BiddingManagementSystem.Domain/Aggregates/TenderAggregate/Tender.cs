using System;
using System.Collections.Generic;
using System.Linq;
using BiddingManagementSystem.Domain.Common;
using BiddingManagementSystem.Domain.Enums;
using BiddingManagementSystem.Domain.Events;
using BiddingManagementSystem.Domain.Exceptions;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Domain.Aggregates.TenderAggregate
{
    public class Tender : BaseEntity, IAggregateRoot
    {
        public string Title { get; private set; }
        public string ReferenceNumber { get; private set; }
        public string Description { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime ClosingDate { get; private set; }
        public TenderType Type { get; private set; }
        public string Category { get; private set; }
        public Money BudgetRange { get; private set; }
        public string ContactEmail { get; private set; }
        public string IssuedByOrganization { get; private set; }
        public TenderStatus Status { get; private set; }
        public Guid? WinningBidId { get; private set; }
        
        private readonly List<TenderDocument> _documents = new();
        public IReadOnlyCollection<TenderDocument> Documents => _documents.AsReadOnly();
        
        private readonly List<EligibilityCriteria> _eligibilityCriteria = new();
        public IReadOnlyCollection<EligibilityCriteria> EligibilityCriteria => _eligibilityCriteria.AsReadOnly();
        
        private readonly List<TenderDeliverable> _deliverables = new();
        public IReadOnlyCollection<TenderDeliverable> Deliverables => _deliverables.AsReadOnly();
        
        private readonly List<TenderActivity> _timeline = new();
        public IReadOnlyCollection<TenderActivity> Timeline => _timeline.AsReadOnly();
        
        private readonly List<TenderPaymentTerm> _paymentTerms = new();
        public IReadOnlyCollection<TenderPaymentTerm> PaymentTerms => _paymentTerms.AsReadOnly();
        
        private Tender() { }
        
        public Tender(
            string title,
            string referenceNumber,
            string description,
            DateTime issueDate,
            DateTime closingDate,
            TenderType type,
            string category,
            Money budgetRange,
            string contactEmail,
            string issuedByOrganization)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));
                
            if (string.IsNullOrWhiteSpace(referenceNumber))
                throw new ArgumentException("Reference number cannot be empty", nameof(referenceNumber));
                
            if (closingDate <= issueDate)
                throw new InvalidTenderDateException("Closing date must be after issue date");
                
            Title = title;
            ReferenceNumber = referenceNumber;
            Description = description;
            IssueDate = issueDate;
            ClosingDate = closingDate;
            Type = type;
            Category = category;
            BudgetRange = budgetRange;
            ContactEmail = contactEmail;
            IssuedByOrganization = issuedByOrganization;
            Status = TenderStatus.Draft;
        }
        
        public void AddDocument(string name, string fileUrl, string description, string fileType, long fileSize)
        {
            var document = new TenderDocument(name, fileUrl, description, fileType, fileSize);
            _documents.Add(document);
        }
        
        public void RemoveDocument(Guid documentId)
        {
            var document = _documents.FirstOrDefault(d => d.Id == documentId);
            if (document != null)
            {
                _documents.Remove(document);
            }
        }
        
        public void AddEligibilityCriteria(string criteriaName, string criteriaDescription)
        {
            var criteria = new EligibilityCriteria(criteriaName, criteriaDescription);
            _eligibilityCriteria.Add(criteria);
        }
        
        public void AddDeliverable(string name, string description)
        {
            var deliverable = new TenderDeliverable(name, description);
            _deliverables.Add(deliverable);
        }
        
        public void AddTimelineActivity(string activityName, DateTime expectedDate)
        {
            var activity = new TenderActivity(activityName, expectedDate);
            _timeline.Add(activity);
        }
        
        public void AddPaymentTerm(string description, decimal percentage)
        {
            if (percentage <= 0 || percentage > 100)
                throw new ArgumentException("Percentage must be between 1 and 100", nameof(percentage));
                
            var paymentTerm = new TenderPaymentTerm(description, percentage);
            _paymentTerms.Add(paymentTerm);
            
            // Ensure total percentage doesn't exceed 100%
            var totalPercentage = _paymentTerms.Sum(p => p.Percentage);
            if (totalPercentage > 100)
                throw new InvalidOperationException("Total payment term percentage cannot exceed 100%");
        }
        
        public void Publish()
        {
            if (Status != TenderStatus.Draft)
                throw new InvalidTenderOperationException("Only draft tenders can be published");
                
            if (!_eligibilityCriteria.Any())
                throw new InvalidTenderOperationException("Tender must have at least one eligibility criteria");
                
            if (!_deliverables.Any())
                throw new InvalidTenderOperationException("Tender must have at least one deliverable");
                
            Status = TenderStatus.Published;
            SetUpdatedBy(CreatedBy);
        }
        
        public void StartEvaluation()
        {
            if (Status != TenderStatus.Published)
                throw new InvalidTenderOperationException("Only published tenders can be moved to evaluation");
                
            if (DateTime.UtcNow < ClosingDate)
                throw new InvalidTenderOperationException("Cannot start evaluation before closing date");
                
            Status = TenderStatus.UnderEvaluation;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void Award(Guid bidId)
        {
            if (Status != TenderStatus.UnderEvaluation)
                throw new InvalidTenderOperationException("Only tenders under evaluation can be awarded");
                
            WinningBidId = bidId;
            Status = TenderStatus.Awarded;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void Close()
        {
            if (Status != TenderStatus.Awarded)
                throw new InvalidTenderOperationException("Only awarded tenders can be closed");
                
            Status = TenderStatus.Closed;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void Cancel(string reason)
        {
            if (Status == TenderStatus.Closed || Status == TenderStatus.Cancelled)
                throw new InvalidTenderOperationException("Cannot cancel a closed or already cancelled tender");
                
            Status = TenderStatus.Cancelled;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void SetInitialStatus()
        {
            Status = TenderStatus.Draft;
            SetUpdatedBy(CreatedBy);
        }
    }
}