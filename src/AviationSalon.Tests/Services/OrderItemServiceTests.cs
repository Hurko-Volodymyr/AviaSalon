using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Abstractions.Services;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AviationSalon.Tests.Services
{
    public class OrderItemServiceTests : IDisposable
    {
        private readonly Mock<IRepository<AircraftEntity>> _aircraftRepositoryMock = new Mock<IRepository<AircraftEntity>>();
        private readonly Mock<IRepository<OrderItemEntity>> _orderItemRepositoryMock = new Mock<IRepository<OrderItemEntity>>();
        private readonly Mock<ILogger<OrderItemService>> _loggerMock = new Mock<ILogger<OrderItemService>>();

        private OrderItemService _orderItemService;
        private AircraftEntity _aircraft;
        private OrderItemEntity _orderItem;
        private OrderEntity _order;

        public OrderItemServiceTests()
        {
            _orderItemService = new OrderItemService(_orderItemRepositoryMock.Object, _aircraftRepositoryMock.Object, _loggerMock.Object);

            _aircraft = new AircraftEntity()
            {
                AircraftId = "1",
                Model = "Model123",
                MaxWeaponsCapacity = 5,
                Weapons = new List<WeaponEntity>()
            };

            _order = new OrderEntity()
            {
                OrderId = "1",
                CustomerId = "1",
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalQuantity = 0,
            };

            _orderItem = new OrderItemEntity()
            {
                OrderItemId = "1",
                OrderId = _order.OrderId,
                AircraftId = _aircraft.AircraftId
            };
        }

        public void Dispose()
        {
            _orderItemRepositoryMock.Reset();
            _aircraftRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Fact]
        public async Task CreateOrderItemAsync_ShouldCreateOrderItem_WhenAircraftExists()
        {
            // Arrange
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraft.AircraftId))
                .ReturnsAsync(_aircraft);

            // Act
            var result = await _orderItemService.TryCreateOrderItemAsync(_order.OrderId, _aircraft.AircraftId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CreateOrderItemAsync_ShouldReturnFalse_WhenAircraftNotFound()
        {
            // Arrange
            string nonExistentAircraftId = "nonExistentAircraft";

            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentAircraftId))
                .ReturnsAsync((AircraftEntity)null);

            // Act
            var result = await _orderItemService.TryCreateOrderItemAsync(_order.OrderId, nonExistentAircraftId);

            // Assert
            result.Should().BeFalse();
            _orderItemRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<OrderItemEntity>()), Times.Never);
        }

        [Fact]
        public async Task GetOrderItemsByOrderIdAsync_ShouldReturnOrderItems_WhenOrderItemsExist()
        {
            // Arrange
            var orderItems = new List<OrderItemEntity>        
            {
                _orderItem,
                new OrderItemEntity { OrderItemId = "item1", OrderId = _order.OrderId,},
            };

            _orderItemRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(orderItems);

            // Act
            var result = await _orderItemService.GetOrderItemsByOrderIdAsync(_order.OrderId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetOrderItemsByOrderIdAsync_ShouldReturnEmptyList_WhenNoOrderItemsExist()
        {
            // Arrange
            _orderItemRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<OrderItemEntity>());

            // Act
            var result = await _orderItemService.GetOrderItemsByOrderIdAsync(_order.OrderId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }

}
