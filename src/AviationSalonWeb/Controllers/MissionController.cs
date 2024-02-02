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
        private readonly IAircraftCatalogService _aircraftCatalogService;
        private readonly IWeaponService _weaponService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MissionController(
            ILogger<MissionController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IOrderService orderService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _aircraftCatalogService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _orderService = orderService;
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
            var order = await _orderService.GetOrderDetailsAsync(orderId);

            if (order != null)
            {
                _logger.LogInformation($"Received order with OrderId: {order.OrderId}");

                if (order.OrderItems != null)
                {
                    _logger.LogInformation($"Number of OrderItems: {order.OrderItems.Count}");

                    foreach (var orderItem in order.OrderItems)
                    {
                        _logger.LogInformation($"OrderItem: AircraftId={orderItem.AircraftId}, Quantity={orderItem.Quantity}");
                    }
                }
                else
                {
                    _logger.LogInformation("OrderItems is null");
                }

                return View(order);
            }
            else
            {
                return NotFound();
            }
        }





    }
}


