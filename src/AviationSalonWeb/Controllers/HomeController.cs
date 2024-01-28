using AviationSalon.Core.Abstractions.Services;
using AviationSalonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AviationSalonWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var aircraftList = await _aircraftCatalogService.GetAircraftListAsync();
            ViewData["AircraftList"] = aircraftList;

            return View();
        }



        [HttpGet]
        [Route("aircrafts")]
        public async Task<IActionResult> Aircrafts()
        {
            var aircraftList = await _aircraftCatalogService.GetAircraftListAsync();
            ViewData["AircraftList"] = aircraftList;

            return View();
        }

        [HttpGet]
        [Route("weapons")] 
        public async Task<IActionResult> Weapons()
        {
            var weaponsList = await _weaponService.GetWeaponsListAsync();
            ViewData["WeaponsList"] = weaponsList;

            return View();
        }

        [HttpGet]
        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("error")] 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
