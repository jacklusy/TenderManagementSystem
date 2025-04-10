using System;

namespace BiddingManagementSystem.Domain.Exceptions
{
    public class InvalidMoneyAmountException : Exception
    {
        public InvalidMoneyAmountException(string message) : base(message)
        {
        }
    }
}