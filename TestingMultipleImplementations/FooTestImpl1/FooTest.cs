using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FooTests
{
    using Interfaces;
    using Microsoft.Practices.Unity;

    [TestClass]
    public class FooTest
    {
        private IUnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            this.container = IoCContainer.GetContainer();
        }

        [TestMethod]
        public void TestThreeLetterLength()
        {
            var foo = this.container.Resolve<IFoo>();
            var returnValue = foo.GetLength("Foo");
            Assert.IsTrue(returnValue == 3);
        }
    }
}
