using System;
using System.Text.RegularExpressions;
using BiddingManagementSystem.Domain.Exceptions;

namespace BiddingManagementSystem.Domain.ValueObjects
{
    public class Email
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);
            
        public string Value { get; private set; }
        
        private Email() { }
        
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException("Email cannot be empty");
                
            if (!EmailRegex.IsMatch(value))
                throw new InvalidEmailException("Invalid email format");
                
            Value = value;
        }
        
        public static implicit operator string(Email email) => email.Value;
        
        public override string ToString() => Value;
        
        public override bool Equals(object obj)
        {
            if (obj is not Email other)
                return false;
                
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }
        
        public override int GetHashCode()
        {
            return Value.ToLowerInvariant().GetHashCode();
        }
    }
}