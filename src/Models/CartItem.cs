namespace RolleiShop.Models.Entities
{
    public class CartItem : Entity
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int CatalogItemId { get; set; }
    }
}
