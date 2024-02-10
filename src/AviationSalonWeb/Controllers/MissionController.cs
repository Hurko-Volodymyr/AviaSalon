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
using static AviationSalon.Core.Abstractions.Services.ICustomerService;

namespace AviationSalonWeb.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]

    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IAircraftCatalogService _aircraftService;
        private readonly IWeaponService _weaponService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MissionController(
            ILogger<MissionController> logger,
            IAircraftCatalogService aircraftCatalogService,
            IWeaponService weaponCatalogService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            ICustomerService customerService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _aircraftService = aircraftCatalogService;
            _weaponService = weaponCatalogService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _httpContextAccessor = httpContextAccessor;
            _customerService = customerService;
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

        [HttpPost]
        [Route("editorder")]
        public async Task<IActionResult> EditOrder([FromBody] EditOrderModel model)
        {
            try
            {
                _logger.LogInformation($"Editing order with ID: {model.OrderId}, adding aircraft with ID: {model.SelectedAircraftId}");

                

                var success = await _orderService.TryEditOrderAsync(model.OrderId, model.SelectedAircraftId);

                if (success)
                {
                    _logger.LogInformation($"Order updated successfully");
                    return Json(new { success = true });
                }
                else
                {
                    _logger.LogWarning($"Failed to update order.");
                    return Json(new { success = false, message = "Failed to update order." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating order. Exception: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while updating order.", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("deleteorder")]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            try
            {
                var success = await _orderService.TryDeleteOrderAsync(orderId);

                if (success)
                {
                    _logger.LogInformation("Order canceled successfully!");
                }

                else
                {
                    _logger.LogWarning("Failed to cancel order.");                
                }

                return RedirectToRoute(new { controller = "Home", action = "Index" });

            }
            catch (Exception ex)
            {                
                _logger.LogWarning($"An error occurred while canceling the order: {ex.Message}.");
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }

        [HttpGet]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(string orderId)
        {
            ViewBag.OrderId = orderId;
            _logger.LogInformation($"Go to checkout with order: {orderId}");
            return await Task.FromResult(View());
        }


        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout([FromForm] CustomerInfoModel customerModel, string orderId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.UpdateCustomerDetailsAsync(customerModel.UserSecret, customerModel.Name, customerModel.ContactInformation);
                    var result = await _customerService.TryAddOrderToCustomerAsync(customerModel.UserSecret, orderId);

                    if (result)
                    {
                        _logger.LogInformation($"Order was added to customer successfully");
                    }
                    else
                    {
                        _logger.LogWarning($"Order was not added to customer");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred during checkout. Exception: {ex.Message}");
                    TempData["ErrorMessage"] = "Error occurred during checkout.";
                }
            }

            return View();
        }

        [HttpGet]
        [Route("orderhistory")]
        public async Task<IActionResult> OrderHistory(string customerId)
        {
            _logger.LogInformation($"Go to orders with customerId: {customerId}");
            var orders = await _orderService.GetCustomerOrdersAsync(customerId);
            if (orders == null)
                _logger.LogWarning($"Any order was not added to customer yet");

            return View(orders);
        }

        [HttpGet]
        [Route("orderhistorydetails")]
        public async Task<IActionResult> OrderHistoryDetails(string orderId, string customerId)
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
                    ViewBag.CustomerId = customerId;

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



