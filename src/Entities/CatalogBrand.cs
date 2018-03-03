using System.Collections.Generic;

namespace RolleiShop.Entities
{
    public class CatalogBrand : Entity
    {
        public string Brand { get; private set; }

        private CatalogBrand () {}

        private CatalogBrand (string brand)
        {
            Brand = brand;
        }

        public static CatalogBrand Create (string brand)
        {
            return new CatalogBrand(brand);
        }
    }
}
