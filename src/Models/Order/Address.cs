using System;
using System.Collections.Generic;

namespace RolleiShop.Models.Entities
{
    public class Address
    {
        public String Street { get; private set; }
        public String City { get; private set; }
        public String State { get; private set; }
        public String Country { get; private set; }
        public String ZipCode { get; private set; }

        private Address() { }

        private Address(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        public static Address Create (string street, string city, string state, string country, string zipcode)
        {
            return new Address(street, city, state, country, zipcode);
        }
    }
}
