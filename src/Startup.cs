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

namespace CameraVillage
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup (IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext> (options =>
                options.UseNpgsql (_config.GetConnectionString ("IdentityConnectionString")));

            services.AddIdentity<ApplicationUser, IdentityRole> ()
                .AddEntityFrameworkStores<IdentityDbContext> ()
                .AddDefaultTokenProviders ();

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
