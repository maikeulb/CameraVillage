namespace CameraVillage.Domain.Models
{
    public class CatalogType : Entity
    {
        public string Type { get; set; }

        private CatalogType () {}

        private CatalogType (string type)
        {
            Type = type;
        }

        public static CatalogType Create ( string type)
        {
            return new CatalogType (type);
        }
    }
}
