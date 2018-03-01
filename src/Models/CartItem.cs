namespace RolleiShop.Models.Entities
{
    public class CartItem : Entity
    {
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public int CatalogItemId { get; private set; }

        private CartItem () {}

        private CartItem (decimal unitPrice, int quantity, int catalogItemId)
        {
            UnitPrice = unitPrice;
            Quantity = quantity;
            CatalogItemId = catalogItemId;
        }

        public static CartItem Create (decimal unitPrice, int quantity, int catalogItemId) 
        {
            return new CartItem (unitPrice, quantity, catalogItemId);
        }

        public void UpdateQuantity (int quantity)
        {
            Quantity = quantity;
        }

        public void IncrementQuantity ()
        {
            Quantity += 1;
        }
    }
}
