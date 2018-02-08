namespace RolleiShop.Models.Entities
{
    public class CatalogType : Entity
    {
        public string Type { get; private set; }

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
