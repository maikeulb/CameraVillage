namespace CameraVillage.Domain.Models
{
    public class CatalogItemOrdered : ValueObject<CatalogItemOrdered>
    {
        public int CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUri { get; private set; }

        private CatalogItemOrdered() {}

        private CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
        {
            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
        }

        private static CatalogItemOrdered Create(int catalogItemId, string productName, string pictureUri)
        {
            return new CatalogItemOrdered (catalogItemId, productName, pictureUri);
        }

        public bool IsEmpty () {
          if (string.IsNullOrEmpty (ProductName) &&
              string.IsNullOrEmpty (PictureUri))
          {
            return true;
          } else {
            return false;
          }
        }
    }
}
