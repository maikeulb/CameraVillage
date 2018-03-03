namespace RolleiShop.Entities
{
    public class CatalogItemOrdered 
    {
        public CatalogItemOrdered(int catalogItemId, string productName, string imageUrl)
        {
            CatalogItemId = catalogItemId;
            ProductName = productName;
            ImageUrl = ImageUrl;
        }

        private CatalogItemOrdered() {}

        public int CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string ImageUrl { get; private set; }
    }
}
