using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _CustomerService;

        public CustomersController(CustomerService CustomerService)
        {
            _CustomerService = CustomerService;
        }

        [HttpGet]
        public ActionResult<List<Customer>> Get() =>
            _CustomerService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCustomer")]
        public ActionResult<Customer> Get(string id)
        {
            var Customer = _CustomerService.Get(id);

            if (Customer == null)
            {
                return NotFound();
            }

            return Customer;
        }

        [HttpPost]
        public ActionResult<Customer> Create(Customer Customer)
        {
            _CustomerService.Create(Customer);

            return CreatedAtRoute("GetCustomer", new { id = Customer.Id.ToString() }, Customer);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Customer CustomerIn)
        {
            var Customer = _CustomerService.Get(id);

            if (Customer == null)
            {
                return NotFound();
            }

            _CustomerService.Update(id, CustomerIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var Customer = _CustomerService.Get(id);

            if (Customer == null)
            {
                return NotFound();
            }

            _CustomerService.Remove(Customer.Id);

            return NoContent();
        }
    }
}