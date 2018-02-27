using System;
using System.Collections.Generic;
using System.Text;
using RolleiShop.Models.Entities;
using RolleiShop.Services.Interfaces;

namespace RolleiShop.Models.Entities
{
    public class Buyer : Entity
    {
        public string IdentityGuid { get; private set; }

        protected Buyer () { }

        private Buyer (string identity) : this ()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace (identity) ? identity : throw new ArgumentNullException (nameof (identity));
        }
    }
}
