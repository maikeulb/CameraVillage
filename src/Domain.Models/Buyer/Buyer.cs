using CameraVillage.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CameraVillage.Domain.Models
{
    public class Buyer : Entity, IAggregateRoot
    {
        private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

        public string IdentityGuid { get; private set; }

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        private Buyer() { }

        private Buyer(string identity) : this()
        {
            IdentityGuid = identity;
        }

        public static Buyer Create (string identity)
        {
            return new Buyer (identity);
        }

         // FirstName,
         // LastName
         // Email
         // PassWord
         // Balance
         // Add CreditCard 
    }
}
