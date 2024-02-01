using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure;
using AviationSalon.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Tests.Repositories
{
    public class OrderItemRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;

        private OrderItemRepository _orderItemRepository;
        private OrderEntity _existingOrder;
        private OrderItemEntity _nonExistingOrderItem;
        private OrderItemEntity _orderItem1;
        private OrderItemEntity _orderItem2;

        public OrderItemRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);

            _orderItemRepository = new OrderItemRepository(_dbContext);

            _orderItem1 = new OrderItemEntity
            {
                OrderItemId = "1",
                AircraftId = "1",
                OrderId = "1",
                Quantity = 2,
            };

            _orderItem2 = new OrderItemEntity
            {
                OrderItemId = "2",
                AircraftId = "2",
                OrderId = "1",
                Quantity = 1,
            };

            _existingOrder = new OrderEntity
            {
                OrderId = "1",
                OrderDate = DateTime.Now,
                CustomerId = "12",
                Customer = new CustomerEntity() { CustomerId = "12"},
                OrderItems = new List<OrderItemEntity> { _orderItem1, _orderItem2 },
                TotalQuantity = _orderItem1.Quantity + _orderItem2.Quantity,
                Status = OrderStatus.Pending,
            };

            _nonExistingOrderItem = new OrderItemEntity { OrderItemId = "-12341" };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddOrderItem()
        {
            // Act
            await _orderItemRepository.AddAsync(_orderItem1);

            // Assert
            var orderItemFromDb = await _dbContext.OrderItems.FindAsync(_orderItem1.OrderItemId);
            orderItemFromDb.Should().NotBeNull();
            orderItemFromDb.AircraftId.Should().Be(_orderItem1.AircraftId);
            orderItemFromDb.Quantity.Should().Be(_orderItem1.Quantity);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectOrderItem()
        {
            // Arrange
            await _dbContext.OrderItems.AddAsync(_orderItem1);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderItemRepository.GetByIdAsync(_orderItem1.OrderItemId);

            // Assert
            result.Should().NotBeNull();
            result.AircraftId.Should().Be(_orderItem1.AircraftId);
            result.Quantity.Should().Be(_orderItem1.Quantity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrderItem()
        {
            // Arrange
            await _dbContext.OrderItems.AddAsync(_orderItem1);
            await _dbContext.SaveChangesAsync();
            _orderItem1.OrderId = "10";

            // Act
            await _orderItemRepository.UpdateAsync(_orderItem1);

            // Assert
            var updatedOrderItem = await _dbContext.OrderItems.FindAsync(_orderItem1.OrderItemId);
            updatedOrderItem.Should().NotBeNull();
            updatedOrderItem.OrderId.Should().Be(_orderItem1.OrderId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOrderItem()
        {
            // Arrange
            await _dbContext.OrderItems.AddAsync(_orderItem1);
            await _dbContext.SaveChangesAsync();

            // Act
            await _orderItemRepository.DeleteAsync(_orderItem1);

            // Assert
            var deletedOrderItem = await _dbContext.OrderItems.FindAsync(_orderItem1.OrderItemId);
            deletedOrderItem.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistingOrderItem()
        {
            // Act
            var result = await _orderItemRepository.GetByIdAsync(_nonExistingOrderItem.OrderItemId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForNonExistingOrderItem()
        {
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _orderItemRepository.UpdateAsync(_nonExistingOrderItem));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForOrderItemWithNullId()
        {
            // Arrange
            _nonExistingOrderItem.OrderItemId = null;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderItemRepository.UpdateAsync(_nonExistingOrderItem));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionForNonExistingOrderItem()
        {
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _orderItemRepository.DeleteAsync(_nonExistingOrderItem));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionForOrderItemWithNullId()
        {
            // Arrange
            _nonExistingOrderItem.OrderItemId = null;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderItemRepository.DeleteAsync(_nonExistingOrderItem));
        }

    }
}
