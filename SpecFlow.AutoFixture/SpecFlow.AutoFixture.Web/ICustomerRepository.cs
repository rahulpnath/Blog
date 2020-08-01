using SpecFlow.AutoFixture.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpecFlow.AutoFixture.Web
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> Get(Guid id);
        Task Create(Customer customer);

    }
}
