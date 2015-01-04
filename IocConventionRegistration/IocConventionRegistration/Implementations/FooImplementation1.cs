namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    public class FooImplementation1 : IFoo
    {

        public string GetData()
        {
            return "This is Foo Implementation 1";
        }
    }
}