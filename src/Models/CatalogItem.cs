using RolleiShop.Features.CatalogManager;
using System;

namespace RolleiShop.Models.Entities
{
    public class CatalogItem : Entity
    {
        public int CatalogTypeId { get; private set; }
        public int CatalogBrandId { get; private set; }
        public int AvailableStock { get; private set; }
        public decimal Price { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageName{ get; private set; } 
        public string ImageUrl { get; set; } 
        public DateTime CreatedDate { get; private set; } 

        public CatalogType CatalogType { get; private set; }
        public CatalogBrand CatalogBrand { get; private set; }

        private CatalogItem () {}

        private CatalogItem (
                int catalogTypeId,
                int catalogBrandId,
                int availableStock,
                decimal price,
                string name,
                string description,
                string imageName,
                string imageUrl
                )
        {
            CatalogTypeId = catalogTypeId;
            CatalogBrandId = catalogBrandId;
            AvailableStock = availableStock;
            Price = price;
            Name = name;
            Description = description;
            ImageName = imageName;
            ImageUrl = imageUrl;
        }

        public static CatalogItem Create (
                int catalogTypeId,
                int catalogBrandId,
                int availableStock,
                decimal price,
                string name,
                string description,
                string imageName,
                string imageUrl
                )
        {
            return new CatalogItem (
                catalogTypeId,
                catalogBrandId,
                availableStock,
                price,
                name,
                description,
                imageName,
                imageUrl
                );
        }

        public void UpdateDetails (Edit.Command command)
        {
            AvailableStock = command.Stock;
            Price = command.Price;
            Description = command.Description;
        }

        public int RemoveStock(int quantityDesired)
        {
            int removed = Math.Min(quantityDesired, this.AvailableStock);

            this.AvailableStock -= removed;

            return removed;
        }

        public int AddStock(int quantity)
        {
            int original = this.AvailableStock;

            this.AvailableStock += quantity;

            return this.AvailableStock - original;
        }
    }
}
