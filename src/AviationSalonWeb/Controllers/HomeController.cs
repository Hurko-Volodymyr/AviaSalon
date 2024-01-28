using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalonWeb.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;

namespace AviationSalonWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAircraftCatalogService _aircraftCatalogService;
        private readonly IWeaponService _weaponService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(
            ILogger<HomeController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var aircraftList = await _aircraftCatalogService.GetAircraftListAsync();
            ViewData["AircraftList"] = aircraftList;

            ViewData["Title"] = _localizer["Title"];

            return View();
        }


        [HttpPost]
        [Route("changelanguage")]
        public IActionResult ChangeLanguage(string culture)
        {
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);

                HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during language change. Culture: {Culture}", culture);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("aircraftdetails/{id}")]
        public async Task<IActionResult> AircraftDetails(int id)
        {
            var aircraftDetails = await _aircraftCatalogService.GetAircraftDetailsAsync(id);
            return View(aircraftDetails);
        }

        [HttpGet]
        [Route("toequip/{id}")]
        public async Task<IActionResult> AircraftToEquip(int id)
        {
            var aircraftDetails = await _aircraftCatalogService.GetAircraftDetailsAsync(id);
            var weaponsList = await _weaponService.GetWeaponsListAsync();

            var equipData = new Tuple<AircraftEntity, List<WeaponEntity>>(aircraftDetails, weaponsList);

            return View(equipData);
        }

        [HttpGet]
        [Route("updateweaponcount/{aircraftId}/{weaponId}/{count}")]
        public async Task<IActionResult> UpdateWeaponCount(int aircraftId, int weaponId, int count)
        {
            if (count <= 0)
            {
                return Json(new { success = false, message = "Count should be greater than zero." });
            }

            try
            {
                for (int i = 0; i < count; i++)
                {
                    await _aircraftCatalogService.EquipAircraftWithWeaponAsync(aircraftId, weaponId);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error equipping aircraft with weapon. Details: {ex.Message}" });
            }
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

