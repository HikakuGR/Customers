using BlazorApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMongoCollection<Customer> _Customers;

        public CustomerService(ICustomersDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Customers = database.GetCollection<Customer>(settings.CustomersCollectionName);
        }

        public List<Customer> Get() =>
            _Customers.Find(Customer => true).ToList();

        public Customer Get(string id) =>
            _Customers.Find<Customer>(Customer => Customer.Id == id).FirstOrDefault();

        public List<Customer> GetPaged(int page, int pageSize, out int total)
        {
            var customers = _Customers.Find(Customer => true).Skip(page * pageSize).Limit(pageSize).ToList();
            total = (int)_Customers.CountDocuments(Customer => true);
            return customers;
        }
        public Customer Create(Customer Customer)
        {
            _Customers.InsertOne(Customer);
            return Customer;
        }

        public void Update(string id, Customer CustomerIn) =>
            _Customers.ReplaceOne(Customer => Customer.Id == id, CustomerIn);

        public void Remove(Customer CustomerIn) =>
            _Customers.DeleteOne(Customer => Customer.Id == Customer.Id);

        public void Remove(string id) =>
            _Customers.DeleteOne(Customer => Customer.Id == id);
    }

    public interface ICustomerService
    { 
        public List<Customer> Get();

        public Customer Get(string id);

        public List<Customer> GetPaged(int page, int pageSize, out int total);

        public Customer Create(Customer Customer);

        public void Update(string id, Customer CustomerIn);

        public void Remove(Customer CustomerIn);

        public void Remove(string id);
    }

}
