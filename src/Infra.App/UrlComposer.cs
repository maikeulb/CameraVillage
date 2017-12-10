namespace CameraVillage.Domain.Services
{
    public class UrlComposer : IUrlComposer
    {
        private readonly CatalogSettings _catalogSettings;

        public UrlComposer(CatalogSettings catalogSettings) => _catalogSettings = catalogSettings;

        public string ComposeImageUrl(string urlTemplate)
        {
            return urlTemplate.Replace("http://catalogbaseurl", _catalogSettings.CatalogBaseUrl);
        }
    }
}
