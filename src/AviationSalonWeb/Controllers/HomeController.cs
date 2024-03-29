using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.WebUI.Models;
using AviationSalonWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;

namespace AviationSalonWeb.Controllers
{
    [ApiController]  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAircraftCatalogService _aircraftCatalogService;
        private readonly IWeaponService _weaponService;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(
            ILogger<HomeController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IStringLocalizer<HomeController> localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var aircraftList = await _aircraftCatalogService.GetAircraftListAsync();
            ViewData["AircraftList"] = aircraftList;

            return View();
        }


        [HttpPost]
        [Authorize]
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
        [Authorize]
        [Route("aircraftdetails/{id}")]
        public async Task<IActionResult> AircraftDetails(string id)
        {
            var aircraftDetails = await _aircraftCatalogService.GetAircraftDetailsAsync(id);
            return View(aircraftDetails);
        }

        [HttpGet]
        [Authorize]
        [Route("toequip/{id}")]
        public async Task<IActionResult> AircraftToEquip(string id)
        {
            var aircraftDetails = await _aircraftCatalogService.GetAircraftDetailsAsync(id);
            var weaponsList = await _weaponService.GetWeaponsListAsync();
            var currentUser = _httpContextAccessor.HttpContext.User.Identity.Name;
            _logger.LogInformation($"Select aircraft with id: {id} and user with id = {currentUser}");
            ViewBag.CustomerId = currentUser;
            var equipData = new Tuple<AircraftEntity, List<WeaponEntity>>(aircraftDetails, weaponsList);

            return View(equipData);
        }

        [HttpPost]
        [Authorize]
        [Route("updateweaponcount")]
        public async Task<IActionResult> UpdateWeaponCount([FromBody] UpdateWeaponCountDataModel data)
        {
            if (data.Count < 0)
            {
                return Json(new { success = false, message = "Count should be positive." });
            }

            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    await _aircraftCatalogService.EquipAircraftWithWeaponAsync(data.AircraftId, data.WeaponId);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error equipping aircraft with weapon. Details: {ex.Message}" });
            }
        }



        [HttpGet]
        [Authorize]
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

