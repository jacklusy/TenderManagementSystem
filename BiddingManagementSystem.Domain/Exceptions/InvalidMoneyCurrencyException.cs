using System;

namespace BiddingManagementSystem.Domain.Exceptions
{
    public class InvalidMoneyCurrencyException : Exception
    {
        public InvalidMoneyCurrencyException(string message) : base(message)
        {
        }
    }
}