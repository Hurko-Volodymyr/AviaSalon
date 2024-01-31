using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.WebUI.Models;
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
    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IAircraftCatalogService _aircraftCatalogService;
        private readonly IWeaponService _weaponService;
        private readonly IStringLocalizer<MissionController> _localizer;

        public MissionController(
            ILogger<MissionController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IStringLocalizer<MissionController> localizer)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _localizer = localizer;
        }

        [HttpPost]
        [Route("gotomission")]
        public IActionResult GotoMission([FromBody] MissionDataModel missionData)
        {
            if (missionData.SelectedAircraft.Any())
            {
                _logger.LogInformation($"Selected Aircraft Count: {missionData.SelectedAircraft.Count}");
            }

            if (missionData.SelectedWeapons.Any())
            {
                _logger.LogInformation($"Selected Weapons Count: {missionData.SelectedWeapons.Count}");
            }

            var model = new Tuple<List<AircraftEntity>, List<WeaponEntity>>(missionData.SelectedAircraft, missionData.SelectedWeapons);
            return View("YourViewName", model);
        }



        [HttpGet]
        [Route("editcart")]
        public IActionResult EditCart()
        {
            // Логика для получения информации о корзине (предварительно выбранных самолетов и оружия)
            // и передачи ее на страницу редактирования корзины.
            // ...

            return View();
        }

       
    }
}

