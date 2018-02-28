using Microsoft.AspNetCore.Mvc.Rendering;
using RolleiShop.Features.ManageCatalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolleiShop.Infra.App.Interfaces
{
    public interface ICatalogService
    {
        Task<Details.Result> GetCatalogDetailItem(int catalogItemId);
        Task<IEnumerable<SelectListItem>> GetBrands();
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
