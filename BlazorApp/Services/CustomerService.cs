using BlazorApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<Customer>> Get() =>
           await _Customers.Find(Customer => true).ToListAsync();

        public async Task<Customer> Get(string id) =>
           await _Customers.Find<Customer>(Customer => Customer.Id == id).FirstOrDefaultAsync();

        public async Task<List<Customer>> GetPaged(int page, int pageSize)
        {
            var customers = await _Customers.Find(Customer => true).Skip(page * pageSize).Limit(pageSize).ToListAsync();
           
            return  customers;
        }

        public async Task<long> GetCustomerCount()
        {
            long total = 0;
            total = await _Customers.CountDocumentsAsync(Customer => true);
            return total; 
        }
        public async Task<Customer> Create(Customer Customer)
        {
            await _Customers.InsertOneAsync(Customer);
            return Customer;
        }

        public async Task Update(string id, Customer CustomerIn)
        {
            await _Customers.ReplaceOneAsync(Customer => Customer.Id == id, CustomerIn);
        }

        public async Task Remove(Customer CustomerIn) =>
            await _Customers.DeleteOneAsync(Customer => Customer.Id == Customer.Id);

        public async Task Remove(string id) =>
            await _Customers.DeleteOneAsync(Customer => Customer.Id == id);
    }

    public interface ICustomerService
    {
        Task<List<Customer>> Get();

        Task<Customer> Get(string id);

        Task<List<Customer>> GetPaged(int page, int pageSize);

        Task<long> GetCustomerCount();
        

        Task<Customer> Create(Customer Customer);

        Task Update(string id, Customer CustomerIn);

        Task Remove(Customer CustomerIn);

        Task Remove(string id);
    }

}
