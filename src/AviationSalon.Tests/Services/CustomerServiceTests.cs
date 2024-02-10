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
    public class CustomerServiceTests : IDisposable
    {       
        private readonly Mock<IRepository<CustomerEntity>> _customerRepositoryMock = new Mock<IRepository<CustomerEntity>>();
        private readonly Mock<IRepository<OrderEntity>> _orderRepositoryMock = new Mock<IRepository<OrderEntity>>();
        private readonly Mock<ILogger<CustomerService>> _loggerMock = new Mock<ILogger<CustomerService>>();

        private CustomerService _customerService;
        private CustomerEntity _customer;

        public CustomerServiceTests()
        {
            _customerService = new CustomerService(_customerRepositoryMock.Object, _orderRepositoryMock.Object, _loggerMock.Object);

            _customer = new CustomerEntity()
            {
                CustomerId = "1",
                Name = "Customer",
                ContactInformation = "CI",
            };
        }

        public void Dispose()
        {
            _orderRepositoryMock.Reset();
            _customerRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Fact]
        public async Task GetCustomerDetailsAsync_ShouldReturnCustomerDetails_WhenCustomerIdExists()
        {
            // Arrange
            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(_customer.CustomerId))
                .ReturnsAsync(_customer);

            // Act
            var result = await _customerService.GetCustomerDetailsAsync(_customer.CustomerId);

            // Assert
            result.Should().NotBeNull();
            result.CustomerId.Should().Be(_customer.CustomerId);
        }

        [Fact]
        public async Task GetCustomerDetailsAsync_ShouldReturnNull_WhenCustomerIdDoesNotExist()
        {
            // Arrange
            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(_customer.CustomerId))
                .ReturnsAsync((CustomerEntity)null);

            // Act
            var result = await _customerService.GetCustomerDetailsAsync(_customer.CustomerId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateCustomerDetailsAsync_ShouldUpdateCustomerDetails_WhenCustomerIdExists()
        {
            // Arrange
            string newName = "NewName";
            string newContactInfo = "NewContactInfo";

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(_customer.CustomerId))
                .ReturnsAsync(_customer);

            // Act
            await _customerService.UpdateCustomerDetailsAsync(_customer.CustomerId, newName, newContactInfo);
            var result = await _customerService.GetCustomerDetailsAsync(_customer.CustomerId);

            // Assert
            result.Name.Should().Be(newName);
            result.ContactInformation.Should().Be(newContactInfo);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(result), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerDetailsAsync_ShouldNotUpdateCustomerDetails_WhenCustomerIdDoesNotExist()
        {
            // Arrange
            string customerId = "nonExistentCustomer";
            string newName = "NewName";
            string newContactInfo = "NewContactInfo";

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync((CustomerEntity)null);

            // Act
            await _customerService.UpdateCustomerDetailsAsync(customerId, newName, newContactInfo);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<CustomerEntity>()), Times.Never);
        }

        [Fact]
        public async Task AddOrderToCustomerAsync_ShouldAddOrderToCustomer_WhenCustomerAndOrderExist()
        {
            // Arrange
            string orderId = "order123";
            var order = new OrderEntity { OrderId = orderId };

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(_customer.CustomerId))
                .ReturnsAsync(_customer);
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _customerService.TryAddOrderToCustomerAsync(_customer.CustomerId, orderId);

            // Assert
            result.Should().BeTrue();
            _customer.Orders.Should().Contain(order);
        }

        [Fact]
        public async Task AddOrderToCustomerAsync_ShouldReturnFalse_WhenCustomerNotFound()
        {
            // Arrange
            string userSecret = "nonExistentUser";
            string orderId = "order123";

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(userSecret))
                .ReturnsAsync((CustomerEntity)null);

            // Act
            var result = await _customerService.TryAddOrderToCustomerAsync(userSecret, orderId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AddOrderToCustomerAsync_ShouldReturnFalse_WhenOrderNotFound()
        {
            // Arrange
            string orderId = "nonExistentOrder";

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(_customer.CustomerId))
                .ReturnsAsync(_customer);
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId))
                .ReturnsAsync((OrderEntity)null);

            // Act
            var result = await _customerService.TryAddOrderToCustomerAsync(_customer.CustomerId, orderId);

            // Assert
            result.Should().BeFalse();
        }
    }

}


