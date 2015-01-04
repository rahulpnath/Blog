using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IocConventionRegistration
{
    class IoCConventionAttribute : Attribute
    {
        public bool ShouldAppendClassName { get; set; }

        public string Name { get; set; }
    }
}
