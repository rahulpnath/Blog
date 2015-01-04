using IocConventionRegistration.Interfaces;

namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;
    
    [IoCConvention(Name = "CustomName1", ShouldAppendClassName = false)]
    public class FooCustomImplementation1 : IFooCustom
    {

        public string GetData()
        {
            return "This is Foo Custom Implementation 1";
        }
    }
}