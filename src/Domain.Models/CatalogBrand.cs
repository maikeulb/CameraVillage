using System.Collections.Generic;

namespace CameraVillage.Domain.Models
{
    public class CatalogBrand : Entity
    {
        public string Brand { get; set; }

        private CatalogBrand () {}

        private CatalogBrand (string brand)
        {
            Brand = brand;
        }

        public static CatalogBrand Create ( string brand)
        {
            return new CatalogBrand(brand);
        }
    }
}
