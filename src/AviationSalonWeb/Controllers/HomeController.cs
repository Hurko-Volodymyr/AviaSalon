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

        public HomeController(ILogger<HomeController> logger, IAircraftCatalogService aircraftCatalogService)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
        }

        public async Task<IActionResult> Index()
        {
            var aircraftList = await _aircraftCatalogService.GetAircraftListAsync();

            return View(aircraftList);
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
