using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure;
using AviationSalon.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public class OrderController : ControllerBase
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ApplicationDbContext _context;

            public OrderController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
            {
                _httpContextAccessor = httpContextAccessor;
                _context = context;
            }

            [HttpPost("CreateOrder")]
            public IActionResult CreateOrder([FromBody] OrderModel orderModel)
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
                    OrderDate = DateTime.Now,
                    CustomerId = customerId,
                    TotalQuantity = orderModel.OrderItems.Sum(item => item.Quantity),
                };

                foreach (var orderItemDto in orderModel.OrderItems)
                {
                    var orderItem = new OrderItemEntity
                    {
                    };

                    order.OrderItems.Add(orderItem);
                }

                _context.Orders.Add(order);
                _context.SaveChanges();

                return Ok(order);
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

