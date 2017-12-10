using CameraVillage.Infra.App;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CameraVillage.Features.Catalog
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService) 
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index (int? brandFilterApplied, int? typesFilterApplied, int? page)
        {
            var itemsPage = 10;
            var catalogModel = await _catalogService.GetCatalogItems(page ?? 0, itemsPage, brandFilterApplied, typesFilterApplied);
            return View(catalogModel);
        }

        public IActionResult Details (int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var catalogModel = _catalogService.GetCatalogDetailItem(id);
            if (catalogModel == null)
                return NotFound();
            return View(catalogModel);
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View();
        }

    }

    public class CatalogIndexViewModel
    {
        public IEnumerable<CatalogItemViewModel> CatalogItems { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public int? BrandFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }

    public class CatalogDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
    }

    public class CatalogItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
    }

    public class PaginationInfoViewModel
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int ActualPage { get; set; }
        public int TotalPages { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
    }
}
