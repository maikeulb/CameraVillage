using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CameraVillage.Infra.App;
using CameraVillage.Infra.Identity;
using CameraVillage.Domain.Services;
using CameraVillage.Domain.Models.Interfaces;
using CameraVillage.Infra.Data.Repositories;
using CameraVillage.Infra.Data.Context;

namespace CameraVillage
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseNpgsql (Configuration.GetConnectionString ("CameraVillage")));

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql (Configuration.GetConnectionString ("Identity")));

            services.AddIdentity<ApplicationUser, IdentityRole> ()
                .AddEntityFrameworkStores<IdentityDbContext> ()
                .AddDefaultTokenProviders ();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddScoped<ICatalogService, CatalogService>();
            services.AddSingleton<IUriComposer>(new UriComposer(Configuration.Get<CatalogSettings>()));

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc (options =>
                {
                    options.Filters.Add (typeof (ValidatorActionFilter));
                })
                .AddFeatureFolders ()
                .AddFluentValidation (cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup> (); });

            services.AddMediatR ();
            services.AddAutoMapper ();
            Mapper.AssertConfigurationIsValid ();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
