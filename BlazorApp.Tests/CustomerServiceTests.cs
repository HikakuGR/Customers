using System;
using Xunit;
using BlazorApp;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using MongoDB.Bson;
using Mongo2Go;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using BlazorApp.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Tests
{


    public class CustomerServiceTests : IDisposable
    {
        MongoDbRunner _runner;
        public CustomerServiceTests()
        {
            _runner = MongoDbRunner.Start();
        }

        private CustomerService CreateCustomerService()
        {
            var customerService = new CustomerService(new CustomersDatabaseSettings
            {
                ConnectionString = _runner.ConnectionString,
                CustomersCollectionName = "Customers",
                DatabaseName = "Test"
            });
            return customerService;
        }

        [Fact]
        public async Task CustomerService_Create_ThenCount()
        {
            var customerService = CreateCustomerService();

            await customerService.Create(new Customer
            {
                ContactName = "customer 1",
                Phone = "4343"
            });

            var customersCount = await customerService.GetCustomerCount();
            Assert.Equal(1, customersCount);
        }

        [Fact]
        public async Task CustomerService_Create_ThenGet()
        {
            var customerService = CreateCustomerService();
            var expectedContactName = "customer 1";
            await customerService.Create(new Customer
            {
                ContactName = expectedContactName,
                Phone = "4343"
            });

            var customers = await customerService.Get();
            var firstCustomer = customers.First();
            Assert.Equal(expectedContactName, firstCustomer.ContactName);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
