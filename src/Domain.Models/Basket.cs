using System.Collections.Generic;
using System.Linq;

namespace CameraVillage.Domain.Models
{
    public class Basket : Entity
    {
        public string BuyerId { get; set; }
        private readonly List<BasketItem> _items = new List<BasketItem>();

        public IEnumerable<BasketItem> Items => _items.AsReadOnly();

        private Basket () {}

        public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                _items.Add(new BasketItem
                    {
                        CatalogItemId = catalogItemId,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    });
            return;
            }
            var existingItem = Items.FirstOrDefault(i => i.CatalogItemId == catalogItemId);
            existingItem.Quantity += quantity;
        }
    }
}
