using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Infrastructure;
using AviationSalon.Infrastructure.Identity;
using AviationSalon.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace AviationSalonWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            var configuration = builder.Configuration;

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            builder.Services.AddScoped<IRepository<OrderEntity>, OrderRepository>();
            builder.Services.AddScoped<IRepository<OrderItemEntity>, OrderItemRepository>();
            builder.Services.AddScoped<IRepository<CustomerEntity>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<AircraftEntity>, AircraftRepository>();
            builder.Services.AddScoped<IRepository<WeaponEntity>, WeaponRepository>();
            builder.Services.AddScoped<DataSeeder>();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IAircraftCatalogService, AircraftCatalogService>();
            builder.Services.AddScoped<IWeaponService, WeaponService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddLogging();

            builder.Services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddRazorPages();
            var supportedCultures = new[]
            {
                new CultureInfo("uk"),
                new CultureInfo("en"),
            };

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AviationSalon", Version = "v1" });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dataSeeder = services.GetRequiredService<DataSeeder>();
                await dataSeeder.SeedDataAsync();
                await dataSeeder.InitializeUsersAsync();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AviationSalon V1");
            });

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("uk"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
