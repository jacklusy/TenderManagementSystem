using System;

namespace BiddingManagementSystem.Domain.Exceptions
{
    public class InvalidTenderDateException : Exception
    {
        public InvalidTenderDateException(string message) : base(message)
        {
        }
    }
}