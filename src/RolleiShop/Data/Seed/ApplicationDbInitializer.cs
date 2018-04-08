using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using RolleiShop.Entities;
using RolleiShop.Data.Context;

namespace RolleiShop.Data.Seed
{
    public class ApplicationDbInitializer
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            ILogger<ApplicationDbInitializer> logger)
        {
            context.Database.EnsureCreated();
            
            if (!context.CatalogBrands.Any())
            {
                context.CatalogBrands.AddRange(
                    GetPreconfiguredCatalogBrands());

                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange(
                    GetPreconfiguredCatalogTypes());

                    await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                context.CatalogItems.AddRange(
                    GetPreconfiguredItems());

                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>()
            {
                CatalogBrand.Create("Rolleiflex"),
                CatalogBrand.Create("Hasselblad"),
                CatalogBrand.Create("Mamiya"),
                CatalogBrand.Create("Fuji")
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                CatalogType.Create("TLR"),
                CatalogType.Create("SLR"),
                CatalogType.Create("Rangefinder")
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredItems() 
        {
            return new List<CatalogItem>()
            {
                CatalogItem.Create(
                        1, 1, 10, 1200, 
                        "Rolleiflex 2.8GX", 
                        "Rolleiflex 2.8GX Camera",
                        "28gx.jpg",
                        "http://catalogbaseurl/images/products/28gx.jpg"),
                CatalogItem.Create(
                        1, 1, 10, 3800, 
                        "Rolleiflex 4.0 FW", 
                        "Rolleiflex 4.0 FW",
                        "wide.jpg",
                        "http://catalogbaseurl/images/products/wide.jpg"),
                CatalogItem.Create(
                        2, 1, 10, 1200, 
                        "Rolleiflex SL66", 
                        "Rolleiflex SL66 Camera",
                        "sl66.jpg",
                        "http://catalogbaseurl/images/products/sl66.jpg"),
                CatalogItem.Create(
                        2, 2, 10, 1200, 
                        "Hasselblad 500cm", 
                        "Hasselblad 500cm Camera",
                        "500cm.jpg",
                        "http://catalogbaseurl/images/products/500cm.jpg"),
                CatalogItem.Create(
                        3, 2, 10, 8000, 
                        "Hasselblad 205fcc", 
                        "Hasselblad 205fcc Camera",
                        "205fcc.jpg",
                        "http://catalogbaseurl/images/products/205fcc.jpg"),
                CatalogItem.Create(
                        2, 2, 10, 1600, 
                        "Hasselblad SWC", 
                        "Hasselblad SWC",
                        "swc.jpg",
                        "http://catalogbaseurl/images/products/swc.jpg"),
                CatalogItem.Create(
                        2, 3, 10, 600, 
                        "Mamiya RZ67",
                        "Mamiya RZ67 Camera",
                        "rz67.jpg",
                        "http://catalogbaseurl/images/products/rz67.jpg"),
                CatalogItem.Create(
                        1, 3, 10, 300, 
                        "Mamiya C330", 
                        "Mamiya C330 Camera",
                        "c330.jpg",
                        "http://catalogbaseurl/images/products/c330.jpg"),
                CatalogItem.Create(
                        3, 3, 10, 1500, 
                        "Mamiya 6", 
                        "Mamiya 6 Camera",
                        "mamiya6.jpg",
                        "http://catalogbaseurl/images/products/mamiya6.jpg"),
                CatalogItem.Create(
                        3, 4, 10, 1200, 
                        "Fuji GF670",
                        "Fuji GF670 Camera",
                        "gf670.jpg",
                        "http://catalogbaseurl/images/products/gf670.jpg"),
                CatalogItem.Create(
                        3, 4, 10, 550, 
                        "Fuji GS645", 
                        "Fuji GS645 Camera", 
                        "gs645.jpg",
                        "http://catalogbaseurl/images/products/gs645.jpg"),
            };
        }
    }
}
