namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    [IoCConvention(Name = "CustomName2", ShouldAppendClassName = false)]
    public class FooCustomImplementation2 : IFooCustom
    {

        public string GetData()
        {
            return "This is Foo Custom Implementation 2";
        }
    }
}