using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _CustomerService;
        

        public CustomersController(ICustomerService CustomerService)
        {
            _CustomerService = CustomerService;
        }

        

        [HttpGet]
        public async Task<ActionResult<List<Customer>>>Get()
        {
            
            return await _CustomerService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetCustomer")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            var Customer = await _CustomerService.Get(id);

            if (Customer == null)
            {
                return NotFound();
            }

            return Customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer Customer)
        {
            await _CustomerService.Create(Customer);

            return CreatedAtRoute("GetCustomer", new { id = Customer.Id.ToString() }, Customer);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Customer CustomerIn)
        {
            var Customer = await _CustomerService.Get(id);

            if (Customer == null)
            {
                return NotFound();
            }

            await _CustomerService.Update(id, CustomerIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Customer = await _CustomerService.Get(id);

            if (Customer == null)
            {
                return NotFound();
            }

            await _CustomerService.Remove(Customer.Id);

            return NoContent();
        }
    }
}