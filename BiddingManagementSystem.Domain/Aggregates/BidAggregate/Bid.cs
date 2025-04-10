using System;
using System.Collections.Generic;
using System.Linq;
using BiddingManagementSystem.Domain.Common;
using BiddingManagementSystem.Domain.Enums;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Domain.Aggregates.BidAggregate
{
    public class Bid : BaseEntity, IAggregateRoot
    {
        public Guid TenderId { get; private set; }
        public Guid BidderId { get; private set; }
        public BidStatus Status { get; private set; }
        public Money BidAmount { get; private set; }
        public DateTime SubmissionDate { get; private set; }
        public string TechnicalProposalSummary { get; private set; }
        public decimal? Score { get; private set; }
        public string EvaluationComments { get; private set; }
        
        private readonly List<BidItem> _bidItems = new();
        public IReadOnlyCollection<BidItem> BidItems => _bidItems.AsReadOnly();
        
        private readonly List<BidDocument> _documents = new();
        public IReadOnlyCollection<BidDocument> Documents => _documents.AsReadOnly();
        
        private Bid() { }
        
        public Bid(
            Guid tenderId,
            Guid bidderId,
            Money bidAmount,
            string technicalProposalSummary)
        {
            TenderId = tenderId;
            BidderId = bidderId;
            BidAmount = bidAmount;
            TechnicalProposalSummary = technicalProposalSummary;
            Status = BidStatus.Draft;
        }
        
        public void AddBidItem(string description, int quantity, Money unitPrice)
        {
            var bidItem = new BidItem(description, quantity, unitPrice);
            _bidItems.Add(bidItem);
            
            // Recalculate total bid amount
            BidAmount = new Money(
                _bidItems.Sum(item => item.TotalPrice.Amount),
                _bidItems.First().TotalPrice.Currency);
        }
        
        public void AddDocument(string name, string fileUrl, string documentType, string fileType, long fileSize)
        {
            var document = new BidDocument(name, fileUrl, documentType, fileType, fileSize);
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
        
        public void Submit()
        {
            if (Status != BidStatus.Draft)
                throw new InvalidOperationException("Only draft bids can be submitted");
                
            if (!_bidItems.Any())
                throw new InvalidOperationException("Bid must include at least one item");
                
            if (!_documents.Any())
                throw new InvalidOperationException("Bid must include supporting documents");
                
            Status = BidStatus.Submitted;
            SubmissionDate = DateTime.UtcNow;
            SetUpdatedBy(BidderId.ToString());
        }
        
        public void StartEvaluation()
        {
            if (Status != BidStatus.Submitted)
                throw new InvalidOperationException("Only submitted bids can be evaluated");
                
            Status = BidStatus.UnderEvaluation;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void SetScore(decimal score, string comments)
        {
            if (score < 0 || score > 100)
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100");
                
            Score = score;
            EvaluationComments = comments;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void Accept()
        {
            if (Status != BidStatus.UnderEvaluation)
                throw new InvalidOperationException("Only bids under evaluation can be accepted");
                
            Status = BidStatus.Accepted;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void Reject(string reason)
        {
            if (Status != BidStatus.UnderEvaluation)
                throw new InvalidOperationException("Only bids under evaluation can be rejected");
                
            Status = BidStatus.Rejected;
            EvaluationComments = reason;
            SetUpdatedBy(UpdatedBy);
        }
        
        public void MarkAsWinner()
        {
            if (Status != BidStatus.Accepted)
                throw new InvalidOperationException("Only accepted bids can be marked as winner");
                
            Status = BidStatus.Winner;
            SetUpdatedBy(UpdatedBy);
        }
    }
}