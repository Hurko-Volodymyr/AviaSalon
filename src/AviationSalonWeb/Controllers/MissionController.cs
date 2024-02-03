using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure;
using AviationSalon.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AviationSalonWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IAircraftCatalogService _aircraftService;
        private readonly IWeaponService _weaponService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MissionController(
            ILogger<MissionController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _aircraftService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] MissionDataModel missionData)
        {
            try
            {
                var userId = missionData.CustomerId;

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated." });
                }

                if (!string.IsNullOrEmpty(missionData.SelectedAircraftId))
                {
                    _logger.LogInformation($"Selected Aircraft Id: {missionData.SelectedAircraftId}");
                }

                _logger.LogInformation($"Before PlaceOrderAsync - Selected Aircraft Id: {missionData.SelectedAircraftId}");

                var aircraftIds = new List<string> { missionData.SelectedAircraftId };
                await _orderService.PlaceOrderAsync(aircraftIds, userId);

                var orderId = await _orderService.PlaceOrderAsync(aircraftIds, userId);
                return Json(new { success = true, orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MissionDetails: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while processing mission details." });
            }
        }

        [HttpGet]
        [Route("missiondetails")]
        public async Task<IActionResult> MissionDetails(string orderId)
        {
            try
            {
                var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);

                if (orderItems != null && orderItems.Count != 0)
                {
                    _logger.LogInformation($"Received order items for OrderId: {orderId}");

                    var aircraftModels = new List<AircraftViewModel>();

                    foreach (var orderItem in orderItems)
                    {
                        var aircraft = await _aircraftService.GetAircraftDetailsAsync(orderItem.AircraftId);

                        _logger.LogInformation($"Received aircraft with weapon counts: {aircraft?.Weapons.Count}");
                        var aircraftModel = new AircraftViewModel
                        {
                            Model = aircraft?.Model ?? "N/A",
                            Quantity = orderItem.Quantity,
                            Weapons = aircraft?.Weapons ?? new List<WeaponEntity>()
                        };

                        aircraftModels.Add(aircraftModel);
                    }

                    ViewBag.OrderId = orderId;

                    return View(aircraftModels);
                }
                else
                {
                    _logger.LogInformation($"No order items found for OrderId: {orderId}");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order items. OrderId: {orderId}. Error: {ex.Message}");
                return StatusCode(500);
            }
        }








    }
}


