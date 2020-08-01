using Microsoft.AspNetCore.Mvc;
using SpecFlow.AutoFixture.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecFlow.AutoFixture.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        public ICustomerRepository CustomerRepository { get; }

        public CustomerController(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            return (await CustomerRepository.GetAll()).ToList() ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(Guid id)
        {
            return await CustomerRepository.Get(id);
        }

        [HttpPost]
        public async Task Create(Customer customer)
        {
            await CustomerRepository.Create(customer);
        }
    }
}
