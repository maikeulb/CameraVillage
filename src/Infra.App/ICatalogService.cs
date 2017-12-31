using Microsoft.AspNetCore.Mvc.Rendering;
using RolleiShop.Features.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Infra.App
{
    public interface ICatalogService
    {
        Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId);
        CatalogDetailViewModel GetCatalogDetailItem(int catalogItemId);
        Task<IEnumerable<SelectListItem>> GetBrands();
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
