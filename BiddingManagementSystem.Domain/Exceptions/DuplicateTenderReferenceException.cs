using System;

namespace BiddingManagementSystem.Domain.Exceptions
{
    public class DuplicateTenderReferenceException : Exception
    {
        public DuplicateTenderReferenceException(string referenceNumber) 
            : base($"A tender with reference number '{referenceNumber}' already exists.")
        {
            ReferenceNumber = referenceNumber;
        }

        public string ReferenceNumber { get; }
    }
} 