﻿using System;
using System.Globalization;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RolleiShop.Data.Context;
using RolleiShop.Identity;
using RolleiShop.Infrastructure;
using RolleiShop.Infrastructure.Interfaces;
using RolleiShop.Services;
using RolleiShop.Services.Interfaces;
using Stripe;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace RolleiShop
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices (IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseNpgsql (Configuration.GetConnectionString ("RolleiShop")));

            services.AddDbContext<IdentityDbContext> (options =>
                options.UseNpgsql (Configuration.GetConnectionString ("Identity")));

            services.AddIdentity<ApplicationUser, IdentityRole> ()
                .AddEntityFrameworkStores<IdentityDbContext> ()
                .AddDefaultTokenProviders ();

            services.AddScoped<ICartService, CartService> ();
            services.AddScoped<ICartViewModelService, CartViewModelService> ();
            services.AddScoped<IOrderService, OrderService> ();
            services.Configure<CatalogSettings> (Configuration);
            services.AddSingleton<IUrlComposer> (new UrlComposer (Configuration.Get<CatalogSettings> ()));

            services.AddTransient<IEmailSender, EmailSender> ();
            services.ConfigureApplicationCookie (options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours (1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });

            services.AddMemoryCache ();

            services.AddDistributedRedisCache (options =>
            {
                options.Configuration = Configuration.GetConnectionString ("Redis");
                options.InstanceName = Configuration.GetSection ("AppSettings").GetValue<string> ("RedisInstanceName");
            });

            services.AddLocalization (options => options.ResourcesPath = "Resources");

            services.AddResponseCaching();

            services.AddMvc (options =>
                {
                    options.Filters.Add (typeof (ValidatorActionFilter));
                    options.Filters.AddService(typeof(TimerAction));
                    options.CacheProfiles.Add("Default",
                        new CacheProfile()
                        {
                            Duration = 30,
                            Location = ResponseCacheLocation.Any
                        });
                    options.CacheProfiles.Add("None",
                        new CacheProfile()
                        {
                            Location = ResponseCacheLocation.None,
                         });
                })
                .AddFeatureFolders ()
                .AddFluentValidation (cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup> (); })
                .AddViewLocalization (LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization ();


            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    Server = Configuration["Email:Server"],
                    Port = Convert.ToInt32(Configuration["Email:Port"]),
                    SenderName = Configuration["Email:SenderName"],
                    SenderEmail = Configuration["Email:SenderEmail"],

                    Account = Configuration["Email:Account"],
                    Password = Configuration["Email:Password"],

                    Security = true
                });
            });

            services.Configure<RequestLocalizationOptions> (options =>
            {
                var supportedCultures = new []
                {
                new CultureInfo ("en-US"),
                new CultureInfo ("ja-JA"),
                };
                options.DefaultRequestCulture = new RequestCulture (culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddMediatR (typeof (Startup));

            services.AddScoped<TimerAction>();

            StripeConfiguration.SetApiKey (Environment.GetEnvironmentVariable ("ASPNETCORE_SECRET_KEY"));
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseResponseCaching();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>> ();
            app.UseRequestLocalization (locOptions.Value);

            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
            }
            else
            {
                app.UseExceptionHandler ("/Home/Error");
            }

            app.UseStaticFiles ();

            app.UseAuthentication ();

            app.UseStatusCodePagesWithReExecute ("/StatusCode/{0}");

            app.UseMvc (routes =>
            {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
