using System;

namespace CameraVillage.Domain.Models
{
    public class CatalogItem : Entity
    {
        public int CatalogTypeId { get; private set; }
        public int CatalogBrandId { get; private set; }
        public int AvailableStock { get; private set; }
        public decimal Price { get; private set; }
        public string Name { get; private set; }
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public string ThumbnailUrl { get; set; } //fix
        public string ImageUrl { get; set; } //fix

        public CatalogType CatalogType { get; private set; }
        public CatalogBrand CatalogBrand { get; private set; }

        private CatalogItem () {}

        private CatalogItem (
                int catalogTypeId,
                int catalogBrandId,
                int availableStock,
                decimal price,
                string name,
                string shortDescription,
                string longDescription,
                string thumbnailUrl,
                string imageUrl
                )
        {
            CatalogTypeId = catalogTypeId;
            CatalogBrandId = catalogBrandId;
            AvailableStock = availableStock;
            Price = price;
            Name = name;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            ThumbnailUrl = thumbnailUrl;
            ImageUrl = imageUrl;
        }

        public static CatalogItem Create (
                int catalogTypeId,
                int catalogBrandId,
                int availableStock,
                decimal price,
                string name,
                string shortDescription,
                string longDescription,
                string thumbnailUrl,
                string imageUrl
                )
        {
            return new CatalogItem (
                catalogTypeId,
                catalogBrandId,
                availableStock,
                price,
                name,
                shortDescription,
                longDescription,
                thumbnailUrl,
                imageUrl
                );
        }

        public int RemoveStock(int quantityDesired)
        {
            /* if (AvailableStock == 0) */
            /* { */
                /* throw new CatalogDomainException($"Empty stock, product item {Name} is sold out"); */
            /* } */

            /* if (quantityDesired <= 0) */
            /* { */
                /* throw new CatalogDomainException($"Item units desired should be greater than cero"); */
            /* } */

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
