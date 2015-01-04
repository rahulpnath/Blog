namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    public class FooImplementation2 : IFoo
    {

        public string GetData()
        {
            return "This is Foo implementation 2";
        }
    }
}