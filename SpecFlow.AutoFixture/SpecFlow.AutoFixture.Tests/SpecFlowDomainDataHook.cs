using BoDi;
using SpecFlow.AutoFixture.Web.Models;
using System;
using TechTalk.SpecFlow;

namespace SpecFlow.AutoFixture.Tests
{

    [Binding]
    public class SpecFlowDomainDataHook
    {
        private readonly IObjectContainer objectContainer;

        public SpecFlowDomainDataHook(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            this.objectContainer.RegisterFactoryAs(a => SetupCustomer());
        }

        private Customer SetupCustomer()
        {
            return new Customer()
            {
                Id = Guid.NewGuid(),
                Age = 27,
                FirstName = "John",
                LastName = "Doe",
                Address = "101 Street, Unknown, 4444"
            };
        }
    }
}
