using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure.Repositories;
using AviationSalon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace AviationSalon.Tests.Repositories
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;

        private OrderRepository _orderRepository;
        private OrderEntity _existingOrder;
        private OrderEntity _nonExistingOrder;
        private CustomerEntity _customer;
        private OrderItemEntity _orderItem1;
        private OrderItemEntity _orderItem2;

        public OrderRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);

            _orderRepository = new OrderRepository(_dbContext);



            _customer = new CustomerEntity
            {
                CustomerId = "1",
                Name = "John Doe",
                ContactInformation = "john@example.com",
            };

            _orderItem1 = new OrderItemEntity
            {
                OrderItemId = "1",
                AircraftId = "1",
                Quantity = 2,
            };

            _orderItem2 = new OrderItemEntity
            {
                OrderItemId = "2",
                AircraftId = "2",
                Quantity = 1,
            };

            _existingOrder = new OrderEntity
            {
                OrderId = "1",
                OrderDate = DateTime.Now,
                CustomerId = _customer.CustomerId,
                Customer = _customer,
                OrderItems = new List<OrderItemEntity> { _orderItem1, _orderItem2 },
                TotalQuantity = _orderItem1.Quantity + _orderItem2.Quantity,
                Status = OrderStatus.Pending,
            };

            _nonExistingOrder = new OrderEntity { OrderId = "-907b7" };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddOrder()
        {
            // Act
            await _orderRepository.AddAsync(_existingOrder);

            // Assert
            var orderFromDb = await _dbContext.Orders.FindAsync(_existingOrder.OrderId);
            orderFromDb.Should().NotBeNull();
            orderFromDb.TotalQuantity.Should().Be(_existingOrder.TotalQuantity);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectOrder()
        {
            // Arrange
            await _dbContext.Orders.AddAsync(_existingOrder);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetByIdAsync(_existingOrder.OrderId);

            // Assert
            result.Should().NotBeNull();
            result.TotalQuantity.Should().Be(_existingOrder.TotalQuantity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrder()
        {
            // Arrange
            await _dbContext.Orders.AddAsync(_existingOrder);
            await _dbContext.SaveChangesAsync();
            _existingOrder.CustomerId = "10";

            // Act
            await _orderRepository.UpdateAsync(_existingOrder);

            // Assert
            var updatedOrder = await _dbContext.Orders.FindAsync(_existingOrder.OrderId);
            updatedOrder.Should().NotBeNull();
            updatedOrder.CustomerId.Should().Be(_existingOrder.CustomerId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOrder()
        {
            // Arrange
            await _dbContext.Orders.AddAsync(_existingOrder);
            await _dbContext.SaveChangesAsync();

            // Act
            await _orderRepository.DeleteAsync(_existingOrder);

            // Assert
            var deletedOrder = await _dbContext.Orders.FindAsync(_existingOrder.OrderId);
            deletedOrder.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistingOrder()
        {
            // Act
            var result = await _orderRepository.GetByIdAsync(_nonExistingOrder.OrderId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForNonExistingOrder()
        {
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _orderRepository.UpdateAsync(_nonExistingOrder));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForOrderWithNullId()
        {
            // Arrange
            _nonExistingOrder.OrderId = null;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderRepository.UpdateAsync(_nonExistingOrder));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionForNonExistingOrder()
        {
            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _orderRepository.DeleteAsync(_nonExistingOrder));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionForOrderWithNullId()
        {
            // Arrange
            _nonExistingOrder.OrderId = null;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderRepository.DeleteAsync(_nonExistingOrder));
        }

    }

}


