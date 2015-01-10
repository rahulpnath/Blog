using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooImplementation1
{
    using System.Diagnostics;

    using Interfaces;

    public class Foo : IFoo
    {
        public int GetLength(string input)
        {
            Console.WriteLine("Implementation 1");
            return input.Length;
        }
    }
}
