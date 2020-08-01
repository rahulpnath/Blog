using AutoFixture;
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
        private readonly Fixture fixture;

        public SpecFlowDomainDataHook(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
            this.fixture = new Fixture();
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            this.objectContainer.RegisterFactoryAs(a => SetupCustomer());
        }

        private Customer SetupCustomer()
        {
            return fixture.Create<Customer>();
        }
    }
}
