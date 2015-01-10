namespace FooTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void InitializeContainerForAssembly(TestContext testContext)
        {
            IoCContainer.InitializeContainer();
        }
         
    }
}