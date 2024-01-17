using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure.Repositories;
using AviationSalon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace AviationSalon.Tests.Repositories
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;

        private CustomerRepository _customerRepository;
        private OrderEntity _existingOrder;
        private OrderEntity _nonExistingOrder;
        private CustomerEntity _nonExistingCustomer;
        private CustomerEntity _customer;
        private OrderItemEntity _orderItem1;
        private OrderItemEntity _orderItem2;

        public CustomerRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);

            _customerRepository = new CustomerRepository(_dbContext);



            _customer = new CustomerEntity
            {
                CustomerId = 1,
                Name = "John Doe",
                ContactInformation = "john@example.com",
            };

            _orderItem1 = new OrderItemEntity
            {
                OrderItemId = 1,
                AircraftId = 1,
                Quantity = 2,
            };

            _orderItem2 = new OrderItemEntity
            {
                OrderItemId = 2,
                AircraftId = 2,
                Quantity = 1,
            };

            _existingOrder = new OrderEntity
            {
                OrderId = 1,
                OrderDate = DateTime.Now,
                CustomerId = _customer.CustomerId,
                Customer = _customer,
                OrderItems = new List<OrderItemEntity> { _orderItem1, _orderItem2 },
                TotalQuantity = _orderItem1.Quantity + _orderItem2.Quantity,
                Status = OrderStatus.Pending,
            };

            _nonExistingCustomer = new CustomerEntity { CustomerId = -12341 };
            _nonExistingOrder = new OrderEntity { OrderId = -12312341 };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddCustomer()
        {
            // Act
            await _customerRepository.AddAsync(_customer);

            // Assert
            var customerFromDb = await _dbContext.Customers.FindAsync(_customer.CustomerId);
            customerFromDb.Should().NotBeNull();
            customerFromDb.Name.Should().Be(_customer.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCustomer()
        {
            // Arrange
            await _dbContext.Customers.AddAsync(_customer);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _customerRepository.GetByIdAsync(_customer.CustomerId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(_customer.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer()
        {
            // Arrange
            await _dbContext.Customers.AddAsync(_customer);
            await _dbContext.SaveChangesAsync();
            _customer.Name = "10";

            // Act
            await _customerRepository.UpdateAsync(_customer);

            // Assert
            var updatedCustomer = await _dbContext.Customers.FindAsync(_customer.CustomerId);
            updatedCustomer.Should().NotBeNull();
            updatedCustomer.Name.Should().Be(_customer.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCustomer()
        {
            // Arrange
            await _dbContext.Customers.AddAsync(_customer);
            await _dbContext.SaveChangesAsync();

            // Act
            await _customerRepository.DeleteAsync(_customer);

            // Assert
            var deletedCustomer = await _dbContext.Customers.FindAsync(_customer.CustomerId);
            deletedCustomer.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistingCustomer()
        {
            // Act
            var result = await _customerRepository.GetByIdAsync(123);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(123)]
        [InlineData(456)]
        public async Task UpdateAsync_ShouldThrowExceptionForNonExistingCustomer(int nonExistingCustomerId)
        {
            // Arrange
            _nonExistingCustomer.CustomerId = nonExistingCustomerId;

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _customerRepository.UpdateAsync(_nonExistingCustomer));
        }

        [Theory]
        [InlineData(123)]
        [InlineData(456)]
        public async Task DeleteAsync_ShouldThrowExceptionForNonExistingCustomer(int nonExistingCustomerId)
        {
            // Arrange
            _nonExistingCustomer.CustomerId = nonExistingCustomerId;

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _customerRepository.DeleteAsync(_nonExistingCustomer));
        }
    }
}
