namespace FooTests
{
    using System;
    using Interfaces;
    using Microsoft.Practices.Unity;

    public static class IoCContainer
    {
        private static IUnityContainer container;

        private static bool isInitialized;

        private const string TestEnviromentVariable = "Foo.tests";

        public static void InitializeContainer()
        {
            if (isInitialized)
            {
                throw new Exception("Container already initialized");
            }

            container = new UnityContainer();
            var test = Environment.GetEnvironmentVariable(TestEnviromentVariable);
           
            if (test == "1")
            {
                container.RegisterType<IFoo, FooImplementation1.Foo>();
            }
            else if (test == "2")
            {
                container.RegisterType<IFoo, FooImplementation2.Foo>();
            }

            isInitialized = true;
        }

        internal static IUnityContainer GetContainer()
        {
            if (!isInitialized)
            {
                throw new Exception("Container not initialized");    
            }

            return container;
        }
    }
}