using AviationSalon.App.Services;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AviationSalon.Tests.Services
{
    public class OrderServiceTests : IDisposable
    {
        private readonly Mock<IRepository<AircraftEntity>> _aircraftRepositoryMock = new Mock<IRepository<AircraftEntity>>();
        private readonly Mock<IRepository<OrderEntity>> _orderRepositoryMock = new Mock<IRepository<OrderEntity>>();
        private readonly Mock<ILogger<OrderService>> _loggerMock = new Mock<ILogger<OrderService>>();

        private List<AircraftEntity> _aircrafts;
        private OrderService _orderService;
        private AircraftEntity _aircraft;
        private WeaponEntity _weapon;
        private WeaponEntity _secondWeapon;
        private OrderEntity _order;
        private OrderItemEntity _orderItem;
        private CustomerEntity _customer;

        public OrderServiceTests()
        {
            _orderService = new OrderService(_orderRepositoryMock.Object, _aircraftRepositoryMock.Object, _loggerMock.Object);
            _weapon = new WeaponEntity()
            {
                WeaponId = "1",
                Name = "TestWeapon1",
                Type = WeaponType.AirToAir,
                GuidedSystem = GuidedSystemType.Infrared,
                Range = 1000,
                FirePower = 50
            };

            _secondWeapon = new WeaponEntity()
            {
                WeaponId = "2",
                Name = "TestWeapon2",
                Type = WeaponType.AirToAir,
                GuidedSystem = GuidedSystemType.Infrared,
                Range = 1000,
                FirePower = 50
            };

            _aircraft = new AircraftEntity()
            {
                AircraftId = "1",
                Model = "Model123",
                MaxWeaponsCapacity = 5,
                Weapons = new List<WeaponEntity>()
            };

            _aircrafts = new List<AircraftEntity>
            {  _aircraft, };

            _customer = new CustomerEntity()
            {
                CustomerId = "1",
                Name = "Name",
                ContactInformation = "Info"
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
            _orderRepositoryMock.Reset();
            _aircraftRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Fact]
        public async Task PlaceOrderAsync_ShouldPlaceOrder_WhenSelectedAircraftsExist()
        {
            // Arrange
            var selectedAircraftsId = new List<string> { _aircraft.AircraftId, "aircraft2" };
            _aircraftRepositoryMock.SetupSequence(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AircraftEntity())
                .ReturnsAsync(new AircraftEntity());

            // Act
            var result = await _orderService.PlaceOrderAsync(selectedAircraftsId, _customer.CustomerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<OrderEntity>()), Times.Once);
        }

        [Fact]
        public async Task EditOrderAsync_ShouldEditOrder_WhenOrderExistsAndAircraftExists()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(_order.OrderId))
                .ReturnsAsync(_order);
            _aircraftRepositoryMock.Setup(repo => repo.GetByIdAsync(_aircraft.AircraftId))
                .ReturnsAsync(new AircraftEntity());

            // Act
            var result = await _orderService.TryEditOrderAsync(_order.OrderId, _aircraft.AircraftId);

            // Assert
            result.Should().BeTrue();
            _order.OrderItems.Should().HaveCount(1);
            _order.OrderItems[0].Should().NotBeNull();
            _orderRepositoryMock.Verify(repo => repo.UpdateAsync(_order), Times.Once);
        }

        [Fact]
        public async Task EditOrderAsync_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Arrange
            string nonExistentOrderId = "nonExistentOrder";
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentOrderId))
                .ReturnsAsync((OrderEntity)null);

            // Act
            var result = await _orderService.TryEditOrderAsync(nonExistentOrderId, _aircraft.AircraftId);

            // Assert
            result.Should().BeFalse();
            _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>()), Times.Never);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ShouldReturnOrderDetails_WhenOrderExists()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(_order.OrderId))
                .ReturnsAsync(_order);

            // Act
            var result = await _orderService.GetOrderDetailsAsync(_order.OrderId);

            // Assert
            result.Should().NotBeNull();
            result.OrderId.Should().Be(_order.OrderId);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            string nonExistentOrderId = "nonExistentOrder";

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentOrderId))
                .ReturnsAsync((OrderEntity)null);

            // Act
            var result = await _orderService.GetOrderDetailsAsync(nonExistentOrderId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCustomerOrdersAsync_ShouldReturnCustomerOrders_WhenOrdersExistForCustomer()
        {
            // Arrange
            var orders = new List<OrderEntity>
            {
                _order,
                new OrderEntity { OrderId = "order2", CustomerId = _customer.CustomerId }
            };

            _orderRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetCustomerOrdersAsync(_customer.CustomerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetCustomerOrdersAsync_ShouldReturnEmptyList_WhenNoOrdersExistForCustomer()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<OrderEntity>());

            // Act
            var result = await _orderService.GetCustomerOrdersAsync(_customer.CustomerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenOrderExists()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(_order.OrderId))
                .ReturnsAsync(_order);

            // Act
            var result = await _orderService.TryDeleteOrderAsync(_order.OrderId);

            // Assert
            result.Should().BeTrue();
            _orderRepositoryMock.Verify(repo => repo.DeleteAsync(_order), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Arrange
            string nonExistentOrderId = "nonExistentOrder";

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentOrderId))
                .ReturnsAsync((OrderEntity)null);

            // Act
            var result = await _orderService.TryDeleteOrderAsync(nonExistentOrderId);

            // Assert
            result.Should().BeFalse();
            _orderRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<OrderEntity>()), Times.Never);
        }
    }

}
