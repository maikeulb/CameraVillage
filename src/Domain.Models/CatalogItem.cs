namespace CameraVillage.Domain.Models
{
    public class CatalogItem : Entity
    {
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUri { get; set; }
        public decimal Price { get; set; }

        public CatalogType CatalogType { get; set; }
        public CatalogBrand CatalogBrand { get; set; }

        private CatalogItem () {}

        private CatalogItem (
                int catalogTypeId,
                int catalogBrandId,
                string name,
                string description,
                string pictureUri,
                decimal price,
                CatalogType catalogType,
                CatalogBrand catalogBrand
                )
        {
            CatalogTypeId = catalogTypeId;
            CatalogBrandId = catalogBrandId;
            Name = name;
            Description = description;
            PictureUri = pictureUri;
            Price = price;
            CatalogType = catalogType;
            CatalogBrand  = catalogBrand;
        }

        public static CatalogItem Create (
                int catalogTypeId,
                int catalogBrandId,
                string name,
                string description,
                string pictureUri,
                decimal price,
                CatalogType catalogType,
                CatalogBrand catalogBrand
                )
        {
            return new CatalogItem (
                catalogTypeId,
                catalogBrandId,
                name,
                description,
                pictureUri,
                price,
                catalogType,
                catalogBrand);
        }
    }
}
