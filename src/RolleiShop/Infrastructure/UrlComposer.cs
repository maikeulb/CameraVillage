using RolleiShop.Infrastructure.Interfaces;

namespace RolleiShop.Infrastructure
{
    public class UrlComposer : IUrlComposer
    {
        private readonly CatalogSettings _catalogSettings;

        public UrlComposer(CatalogSettings catalogSettings) => _catalogSettings = catalogSettings;

        public string ComposeImgUrl(string urlTemplate)
        {
            return urlTemplate.Replace("http://catalogbaseurl", _catalogSettings.CatalogBaseUrl);
        }
    }
}
