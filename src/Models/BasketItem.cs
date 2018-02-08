namespace RolleiShop.Models.Entities
{
    public class BasketItem : Entity
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int CatalogItemId { get; set; }

    }
}
