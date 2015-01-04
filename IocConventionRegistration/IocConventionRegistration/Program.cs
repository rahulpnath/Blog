using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocConventionRegistration
{
    using IocConventionRegistration.Interfaces;

    using Microsoft.Practices.Unity;

    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer unityContainer = new UnityContainer();
            IoCConvention.RegisterByConvention(unityContainer);

            var foobar = unityContainer.Resolve<IFooBar>();
            Console.WriteLine(foobar.GetData());

            var fooFactory = unityContainer.Resolve<IFooFactory>();
            var foo1 = fooFactory.Create("1");
            var foo2 = fooFactory.Create("2");
            
            Console.WriteLine(foo1.GetData());
            Console.WriteLine(foo2.GetData());

            var fooCustomFactory = unityContainer.Resolve<IFooCustomFactory>();
            var fooCustom1 = fooCustomFactory.Create("CustomName1");
            var fooCustom2 = fooCustomFactory.Create("CustomName2");

            Console.WriteLine(fooCustom1.GetData());
            Console.WriteLine(fooCustom2.GetData());

            var fooGenericString = unityContainer.Resolve<IFooGeneric<string>>();
            Console.WriteLine(fooGenericString.GetData("dataType"));

            var fooGenericInt = unityContainer.Resolve<IFooGeneric<int>>();
            Console.WriteLine(fooGenericInt.GetData(1));


            Console.ReadLine();
        }
    }
}
