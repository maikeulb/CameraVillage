using System;
using System.Collections.Generic;

namespace CameraVillage.Domain.Models
{
    public class Address : ValueObject<Address>
    {
        public String Street { get; private set; }
        public String City { get; private set; }
        public String State { get; private set; }
        public String Country { get; private set; }
        public String ZipCode { get; private set; }

        private Address() { }

        private Address(
                string street,
                string city,
                string state,
                string country,
                string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        private static Address Create (
                string street,
                string city,
                string state,
                string country,
                string zipcode)
        {
            return new Address(
                    street,
                    city,
                    state,
                    country,
                    zipcode);
        }

        public bool IsEmpty () {
          if (string.IsNullOrEmpty (Street) &&
                  string.IsNullOrEmpty (City) &&
                  string.IsNullOrEmpty (State) &&
                  string.IsNullOrEmpty (Country) &&
                  string.IsNullOrEmpty (ZipCode))
          {
            return true;
          } else {
            return false;
          }
        }
    }
}
