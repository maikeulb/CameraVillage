using Microsoft.AspNetCore.Mvc.Rendering;
using CameraVillage.Features.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CameraVillage.Infra.App
{
    public interface ICatalogService
    {
        Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId);
        Task<IEnumerable<SelectListItem>> GetBrands();
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
