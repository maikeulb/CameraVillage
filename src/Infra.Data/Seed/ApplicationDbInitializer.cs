using Microsoft.AspNetCore.Builder;
using CameraVillage.Domain.Models;
using CameraVillage.Infra.Data.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraVillage.Infra.Data.Seed
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
                CatalogItem.Create(1, 1, "Rolleiflex 2.8E", "Rolleiflex 2.8E", 19.5M, "http://catalogbaseurltobereplaced/images/products/1.png"),
                CatalogItem.Create(1, 1, "Rolleiflex T", "Rolleiflex T", 19.5M, "http://catalogbaseurltobereplaced/images/products/2.png") ,
                CatalogItem.Create(2, 1, "Rolleiflex SL66", "Rolleiflex SL66", 8.50M, "http://catalogbaseurltobereplaced/images/products/3.png") ,
                CatalogItem.Create(2, 2, "Hasselblad 500cm", "Hasselblad 500cm", 12, "http://catalogbaseurltobereplaced/images/products/4.png") ,
                CatalogItem.Create(3, 2, "Hasselblad 203fe", "Hasselblad 203fe", 12, "http://catalogbaseurltobereplaced/images/products/5.png") ,
                CatalogItem.Create(2, 2, "Hasselblad SWA", "Hasselblad SWA", 12, "http://catalogbaseurltobereplaced/images/products/6.png") ,
                CatalogItem.Create(2, 3, "Mamiya RB67", "Mamiya RB67", 12, "http://catalogbaseurltobereplaced/images/products/7.png") ,
                CatalogItem.Create(1, 3, "Mamiya C330", "Mamiya C330", 8.5M, "http://catalogbaseurltobereplaced/images/products/8.png"), 
                CatalogItem.Create(3, 3, "Mamiya 6", "Mamiya 6", 12, "http://catalogbaseurltobereplaced/images/products/9.png") ,
                CatalogItem.Create(2, 4, "Fuji GX680", "Fuji GX680", 12, "http://catalogbaseurltobereplaced/images/products/10.png") , 
                CatalogItem.Create(3, 4, "Fuji GF670", "Fuji GF670", 8.5M, "http://catalogbaseurltobereplaced/images/products/11.png"),
                CatalogItem.Create(3, 4, "Fuji GS645", "Fuji GS645", 8.5M, "http://catalogbaseurltobereplaced/images/products/12.png") 
            };
        }
    }
}
