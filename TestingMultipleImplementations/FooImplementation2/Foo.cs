using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooImplementation2
{
    using Interfaces;

    public class Foo : IFoo
    {
        public int GetLength(string input)
        {
            Console.WriteLine("Implementation 2");
            return input.Count();
        }
    }
}
