using BlazorApp.Models;
using Mongo2Go;
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

        public async Task<List<Customer>> GetPaged(int page, int pageSize ,string filterText = "")
        {
            if (filterText == null)
            {
                var customers = await _Customers.Find(Customer => true).Skip(page * pageSize).Limit(pageSize).ToListAsync();
                return customers;
            }
            else
            {
                var builder = Builders<Customer>.Filter;
                var filter = builder.Where(Customer => Customer.Address.Contains(filterText)
                || Customer.CompanyName.Contains(filterText)
                || Customer.ContactName.Contains(filterText)
                || Customer.Country.Contains(filterText)
                || Customer.Phone.Contains(filterText)
                || Customer.Region.Contains(filterText)
                || Customer.PostalCode.Contains(filterText)
                || Customer.City.Contains(filterText));
                var customers = await _Customers.Find(filter)
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync();
                return customers;
            }

            
        }

        
        public async Task<long> GetCustomerCount(string filterText="")
        {
            
            long total = await _Customers.Find(Customer => Customer.Address.Contains(filterText)
                || Customer.CompanyName.Contains(filterText)
                || Customer.ContactName.Contains(filterText)
                || Customer.Country.Contains(filterText)
                || Customer.Phone.Contains(filterText)
                || Customer.Region.Contains(filterText)
                || Customer.PostalCode.Contains(filterText)
                || Customer.City.Contains(filterText)).CountDocumentsAsync();
            
            
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

        Task<List<Customer>> GetPaged(int page, int pageSize, string filterText = null);       


        Task<long> GetCustomerCount(string filterText = "");


        Task<Customer> Create(Customer Customer);

        Task Update(string id, Customer CustomerIn);

        Task Remove(Customer CustomerIn);

        Task Remove(string id);
    }

}
