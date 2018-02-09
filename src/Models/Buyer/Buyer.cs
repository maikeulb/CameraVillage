using Rolleishop.Services.Interfaces;
using Rolleishop.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolleiShop.Models.Entities.Buyer
{
    public class Buyer : Entity
    {
        public string IdentityGuid { get; private set; }

        private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        protected Buyer() {}

        public Buyer(string identity) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        }
    }
}
