namespace CameraVillage.Domain.Models
{
    public class BasketItem : Entity
    {
        public int CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public BasketItem () {}
    
        // customer ID
        // BasketID
        // Cost
        // Tax
    }
}
