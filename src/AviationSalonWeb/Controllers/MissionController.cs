using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure;
using AviationSalon.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Route("missiondetails")]
        public async Task<IActionResult> MissionDetails([FromBody] MissionDataModel missionData)
        {
            try
            {
                var userId = "CustomerId1";

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

                await _orderService.PlaceOrderAsync(missionData.SelectedAircraft, userId);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MissionDetails: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while processing mission details." });
            }
        }



        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            if (orderModel == null || orderModel.OrderItems == null || !orderModel.OrderItems.Any())
            {
                return BadRequest("Invalid order data.");
            }

            var customerId = _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;

            if (customerId == null)
            {
                return BadRequest("Unable to retrieve customer id.");
            }

            var order = new OrderEntity
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = DateTime.UtcNow,
                CustomerId = customerId,
                TotalQuantity = orderModel.OrderItems.Sum(item => item.Quantity),
            };

            foreach (var orderItemDto in orderModel.OrderItems)
            {
                var orderItem = new OrderItemEntity
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                };

                order.OrderItems.Add(orderItem);
            }

          //  await _orderService.PlaceOrderAsync(order);

            return Ok(order);
        }        
    }
}


