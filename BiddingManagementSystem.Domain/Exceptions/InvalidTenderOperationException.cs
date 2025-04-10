using System;

namespace BiddingManagementSystem.Domain.Exceptions
{
    public class InvalidTenderOperationException : Exception
    {
        public InvalidTenderOperationException(string message) : base(message)
        {
        }
    }
}