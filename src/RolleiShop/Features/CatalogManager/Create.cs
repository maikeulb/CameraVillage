using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RolleiShop.Data.Context;
using RolleiShop.Entities;

namespace RolleiShop.Features.CatalogManager
{
    public class Create
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            [Display(Name = "Type")]
            public int TypeId { get; set; }
            [Display(Name = "Brand")]
            public int BrandId { get; set; }
            public int Stock { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
            public decimal Price { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public IFormFile ImageUpload { get; set; }
            public string ImageName { get; set; }
            public string ImageUrl { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.TypeId).NotNull();
                RuleFor(m => m.BrandId).NotNull();
                RuleFor(m => m.Stock).NotNull();
                RuleFor(m => m.Price).NotNull();
                RuleFor(m => m.Name).NotNull();
                RuleFor(m => m.Description).NotNull();
                RuleFor(m => m.ImageUpload).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IHostingEnvironment _environment;
            private readonly ILogger _logger;

            public Handler(IHostingEnvironment environment,
                    ApplicationDbContext context,
                    ILogger<Create.Handler> logger)
            {
                _environment = environment;
                _context = context;
                _logger = logger;
            }

            protected override async Task HandleCore(Command message)
            {
                var uploadPath = Path.Combine (_environment.WebRootPath, "images/products");
                var ImageName = ContentDispositionHeaderValue.Parse (message.ImageUpload.ContentDisposition).FileName.Trim ('"');

                using (var fileStream = new FileStream (Path.Combine (uploadPath, message.ImageUpload.FileName), FileMode.Create))
                {
                    await message.ImageUpload.CopyToAsync (fileStream);
                    message.ImageUrl = "http://catalogbaseurl/images/products/" + ImageName;
                }

                var item = CatalogItem.Create (
                    message.TypeId,
                    message.BrandId,
                    message.Stock,
                    message.Price,
                    message.Name,
                    message.Description,
                    ImageName,
                    message.ImageUrl
                );
                _context.CatalogItems.Add (item);
                await _context.SaveChangesAsync ();
            }
        }
    }
}
