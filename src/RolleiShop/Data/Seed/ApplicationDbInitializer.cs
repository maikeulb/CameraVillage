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
                        1, 1, 10, 19.5M, 
                        "Rolleiflex 2.8E", 
                        "Rolleiflex 2.8E Camera",
                        "1.png",
                        "http://catalogbaseurl/images/products/1.png"),
                CatalogItem.Create(
                        1, 1, 10, 19.5M, 
                        "Rolleiflex T", 
                        "Rolleiflex T Camera",
                        "2.png",
                        "http://catalogbaseurl/images/products/2.png"),
                CatalogItem.Create(
                        2, 1, 10, 8.5M, 
                        "Rolleiflex SL66", 
                        "Rolleiflex SL66 Camera",
                        "3.png",
                        "http://catalogbaseurl/images/products/3.png"),
                CatalogItem.Create(
                        2, 2, 10, 12, 
                        "Hasselblad 500cm", 
                        "Hasselblad 500cm Camera",
                        "4.png",
                        "http://catalogbaseurl/images/products/4.png"),
                CatalogItem.Create(
                        3, 2, 10, 100, 
                        "Hasselblad 203fe", 
                        "Hasselblad 203fe Camera",
                        "5.png",
                        "http://catalogbaseurl/images/products/5.png"),
                CatalogItem.Create(
                        2, 2, 10, 15, 
                        "Hasselblad SWA", 
                        "Hasselblad SWA Camera",
                        "6.png",
                        "http://catalogbaseurl/images/products/6.png"),
                CatalogItem.Create(
                        2, 3, 10, 13, 
                        "Mamiya RB67",
                        "Mamiya RB67 Camera",
                        "7.png",
                        "http://catalogbaseurl/images/products/7.png"),
                CatalogItem.Create(
                        1, 3, 10, 11, 
                        "Mamiya C330", 
                        "Mamiya C330 Camera",
                        "8.png",
                        "http://catalogbaseurl/images/products/8.png"),
                CatalogItem.Create(
                        3, 3, 10, 12, 
                        "Mamiya 6", 
                        "Mamiya 6 Camera",
                        "9.png",
                        "http://catalogbaseurl/images/products/9.png"),
                CatalogItem.Create(
                        2, 4, 10, 9.5M, 
                        "Fuji GX680",
                        "Fuji GX680 Camera", 
                        "10.png",
                        "http://catalogbaseurl/images/products/10.png"),
                CatalogItem.Create(
                        3, 4, 10, 8.5M, 
                        "Fuji GF670",
                        "Fuji GF670 Camera",
                        "11.png",
                        "http://catalogbaseurl/images/products/11.png"),
                CatalogItem.Create(
                        3, 4, 10, 8.5M, 
                        "Fuji GS645", 
                        "Fuji GS645 Camera", 
                        "12.png",
                        "http://catalogbaseurl/images/products/12.png"),
            };
        }
    }
}
