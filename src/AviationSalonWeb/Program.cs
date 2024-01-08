using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Infrastructure;
using AviationSalon.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddScoped<IRepository<OrderEntity>, OrderRepository>();
            builder.Services.AddScoped<IRepository<OrderItemEntity>, OrderItemRepository>();
            builder.Services.AddScoped<IRepository<CustomerEntity>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<AircraftEntity>, AircraftRepository>();
            builder.Services.AddScoped<IRepository<WeaponEntity>, WeaponRepository>();

            builder.Services.AddScoped<IOrderService, OrderService>();
            //builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IAircraftCatalogService, AircraftCatalogService>();
            builder.Services.AddScoped<IWeaponService, WeaponService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();
            var supportedCultures = new[] 
            {
                new CultureInfo("en"),
                new CultureInfo("ua")
            };

            var app = builder.Build();

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
