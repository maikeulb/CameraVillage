namespace CameraVillage.Domain.Models
{
    public class OrderItem : Entity
    {
        public CatalogItemOrdered ItemOrdered { get; private set; }
        public int Units { get; private set; }
        public decimal UnitPrice { get; private set; }

        private OrderItem() { }

        private OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            ItemOrdered = itemOrdered;
            UnitPrice = unitPrice;
            Units = units;
        }

        private static OrderItem Create (CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            return new OrderItem(itemOrdered, unitPrice, units);
        }
    }
}
