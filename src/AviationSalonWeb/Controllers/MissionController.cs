using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.WebUI.Models;
using AviationSalonWeb.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace AviationSalonWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IAircraftCatalogService _aircraftCatalogService;
        private readonly IWeaponService _weaponService;
        private readonly IOrderService _orderService;

        public MissionController(
            ILogger<MissionController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IOrderService orderService)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _orderService = orderService;
        }

        [HttpPost]
        [Route("missiondetails")]
        public async Task<IActionResult> MissionDetails([FromBody] MissionDataModel missionData)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated." });
                }

                if (missionData.SelectedAircraft.Any())
                {
                    _logger.LogInformation($"Selected Aircraft Count: {missionData.SelectedAircraft.Count}");
                }

                if (missionData.SelectedWeapons.Any())
                {
                    _logger.LogInformation($"Selected Weapons Count: {missionData.SelectedWeapons.Count}");
                }

                var order = await CreateOrderAsync(missionData.SelectedAircraft, userId);

                return Json(new { success = true, orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MissionDetails: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while processing mission details." });
            }
        }


        private async Task<OrderEntity> CreateOrderAsync(List<AircraftEntity> selectedAircraft, string customerId)
        {
            var order = new OrderEntity
            {
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                CustomerId = customerId, 
            };

            foreach (var aircraft in selectedAircraft)
            {
                var orderItem = new OrderItemEntity
                {
                    AircraftId = aircraft.AircraftId,
                    Quantity = 1,
                };

                order.OrderItems.Add(orderItem);
            }

            await _orderService.PlaceOrderAsync(order);

            return order;
        }





    }
}

