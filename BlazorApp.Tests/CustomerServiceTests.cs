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
         private async Task SeedDBAsync(CustomerService customerService)
        {
            for(int i=0;i<20;i++)
            {
                 await customerService.Create(new Customer
                {
                    ContactName = "customer "+ i,
                    Phone = "4343"+i
                });
            }

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

        [Fact]
        public async Task CustomerService_Create_ThenRemoveWithId_ThenCount()
        {
            var customerService = CreateCustomerService();            
            
            await customerService.Create(new Customer
            {
                
                ContactName = "customer 1",
                Phone = "4343"
            } );

            var customerToRemove = await customerService.Create(new Customer
            {
                
                ContactName = "customer 2",
                Phone = "43434"
            });

            
            await customerService.Remove(customerToRemove.Id);
            var customersCount = await customerService.GetCustomerCount();
            Assert.Equal(1, customersCount);
        }

        [Fact]
        public async Task CustomerService_Seed_GetPaged_ThenVerify()
        {
            var customerService = CreateCustomerService();
            await SeedDBAsync(customerService);
            var result = await customerService.GetPaged(2, 5);
            Assert.Equal(5, result.Count());
            Assert.NotNull(result.FirstOrDefault(c => c.ContactName == "customer 10").ContactName);
            Assert.NotNull(result.FirstOrDefault(c => c.ContactName == "customer 11").ContactName);
            Assert.NotNull(result.FirstOrDefault(c => c.ContactName == "customer 12").ContactName);
            Assert.NotNull(result.FirstOrDefault(c => c.ContactName == "customer 13").ContactName);
            Assert.NotNull(result.FirstOrDefault(c => c.ContactName == "customer 14").ContactName);
        }

        [Fact]
        public async Task CustomerService_Seed_Update_GetBack_Verify()
        {
            var customerService = CreateCustomerService();
            await SeedDBAsync(customerService);
            var customers = await customerService.Get();
            var customerToUpdate = customers.First();
            customerToUpdate.ContactName = "UpdatedCustomer";                
            await customerService.Update(customerToUpdate.Id, customerToUpdate);
            var updatedCustomer = await customerService.Get(customerToUpdate.Id);
            Assert.Equal("UpdatedCustomer", updatedCustomer.ContactName);

        }
        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
