using SpecFlow.AutoFixture.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecFlow.AutoFixture.Web
{
    public class InMemoryCustomerRespository : ICustomerRepository
    {
        private List<Customer> customers = new List<Customer>();

        public Task Create(Customer customer)
        {
            if (customers.Any(a => a.Id == customer.Id))
                throw new Exception("Customer with same Id already exists");

            customers.Add(customer);

            return Task.CompletedTask;
        }

        public Task<Customer> Get(Guid id)
        {
            var customer = customers.FirstOrDefault(a => a.Id == id);
            if (customer == null)
                throw new Exception($"Customer with Id {id} does not exists");

            return Task.FromResult(customer);
        }

        public Task<IEnumerable<Customer>> GetAll()
        {
            return Task.FromResult(customers.AsEnumerable());
        }
    }
}
