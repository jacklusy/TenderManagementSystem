using System;

namespace BiddingManagementSystem.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        
        private Address() { }
        
        public Address(string street, string city, string state, string country, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not Address other)
                return false;
                
            return Street == other.Street && 
                   City == other.City && 
                   State == other.State && 
                   Country == other.Country && 
                   ZipCode == other.ZipCode;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, Country, ZipCode);
        }
    }
}