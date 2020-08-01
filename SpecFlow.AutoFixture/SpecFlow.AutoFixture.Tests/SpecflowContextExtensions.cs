using TechTalk.SpecFlow;

namespace SpecFlow.AutoFixture.Tests
{
    public static class ScenarioContextExtensions
    {
        private const string Actual = "actual";

        public static void AddActual(this ScenarioContext scenarioContext, object value)
        {
            scenarioContext.Add(Actual, value);
        }

        public static T GetActual<T>(this ScenarioContext scenarioContext)
        {
            return scenarioContext.Get<T>(Actual);
        }
    }
}
