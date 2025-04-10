using System;
using BiddingManagementSystem.Domain.Exceptions;

namespace BiddingManagementSystem.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new InvalidMoneyAmountException("Money amount cannot be negative");
                
            if (string.IsNullOrWhiteSpace(currency))
                throw new InvalidMoneyCurrencyException("Currency code is required");
                
            Amount = amount;
            Currency = currency;
        }
        
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies");
                
            return new Money(left.Amount + right.Amount, left.Currency);
        }
        
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies");
                
            return new Money(left.Amount - right.Amount, left.Currency);
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not Money other)
                return false;
                
            return Amount == other.Amount && Currency == other.Currency;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}