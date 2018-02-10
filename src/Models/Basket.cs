using System.Collections.Generic;
using System.Linq;

namespace RolleiShop.Models.Entities
{
    public class Basket : Entity
    {
        private readonly List<BasketItem> _items = new List<BasketItem>();

        public string BuyerId { get; set; }
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                _items.Add(new BasketItem()
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

        public void RemoveItem(int catalogItemId)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                /* _items = _items.Where(item => item.Id != id).ToList(); */
                _items.Remove(_items.Where(item => item.Id == catalogItemId).First());
                return;
            }
        }

    }
}
