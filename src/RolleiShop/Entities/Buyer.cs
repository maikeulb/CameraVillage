using System;

namespace RolleiShop.Entities
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
