using AviationSalon.Core.Abstractions.Services;
using AviationSalonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AviationSalonWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAircraftCatalogService _aircraftCatalogService;
        private readonly IWeaponService _weaponService;

        public HomeController(ILogger<HomeController> logger, IAircraftCatalogService aircraftCatalogService, IWeaponService weaponCatalogService)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
        }

        public async Task<IActionResult> Index()
        {
            var aircraftList = await _aircraftCatalogService.GetAircraftListAsync();
            var weaponsList = await _weaponService.GetWeaponsListAsync();
            ViewData["AircraftList"] = aircraftList;
            ViewData["WeaponsList"] = weaponsList;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
