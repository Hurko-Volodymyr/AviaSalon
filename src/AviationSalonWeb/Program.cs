using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Infrastructure;
using AviationSalon.Infrastructure.Identity;
using AviationSalon.Infrastructure.Repositories;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace AviationSalonWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryClients(IdentityServerConfig.Clients)
                .AddAspNetIdentity<IdentityUser>();

            builder.Services.AddScoped<IRepository<OrderEntity>, OrderRepository>();
            builder.Services.AddScoped<IRepository<OrderItemEntity>, OrderItemRepository>();
            builder.Services.AddScoped<IRepository<CustomerEntity>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<AircraftEntity>, AircraftRepository>();
            builder.Services.AddScoped<IRepository<WeaponEntity>, WeaponRepository>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IAircraftCatalogService, AircraftCatalogService>();
            builder.Services.AddScoped<IWeaponService, WeaponService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ua")
            };

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbInitializer = services.GetRequiredService<IDbInitializer>();
                dbInitializer.InitializeAsync();
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
