using Shouldly;
using SpecFlow.AutoFixture.Web.Models;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlow.AutoFixture.Tests.Steps
{
    [Binding]
    public class CustomerSteps
    {
        private readonly TestContextFixture testContextFixture;
        private readonly ScenarioContext context;
        private readonly Customer customer;

        public CustomerSteps(
            TestContextFixture testContextFixture,
            ScenarioContext context,
            Customer customer)
        {
            this.testContextFixture = testContextFixture;
            this.context = context;
            this.customer = customer;
        }

        [Given(@"I POST a valid customer to the API")]
        public async Task GivenIPOSTAValidCustomerToTheAPI()
        {
            using (var client = testContextFixture.CreateClient())
            {
                await client.SendJsonContent(
                    "/customer",
                    HttpMethod.Post,
                    customer);
            }
        }
        [Given(@"I have a valid customer")]
        public void GivenIHaveAValidCustomer()
        {
            customer.ShouldNotBeNull();
        }

        [When(@"I set First name as '(.*)' and Last name as '(.*)'")]
        public void WhenISetFirstNameAsAndLastNameAs(string firstName, string lastName)
        {
            customer.FirstName = firstName;
            customer.LastName = lastName;
        }


        [When(@"I GET the customer using  the API")]
        public async Task WhenIGETTheCustomerUsingTheAPI()
        {
            using (var client = testContextFixture.CreateClient())
            {
                var actual = await client.GetJsonResult<Customer>($"/customer/{customer.Id}");
                context.AddActual(actual.Result);
            }
        }

        [Then(@"the result should be the customer")]
        public void ThenTheResultShouldBeTheCustomer()
        {
            var actual = context.GetActual<Customer>();
            actual.ShouldBeEquivalentTo(customer);
        }

        [Then(@"the FullName must be '(.*)'")]
        public void ThenTheFullNameMustBe(string expected)
        {
            customer.FullName.ShouldBe(expected);
        }

    }
}
