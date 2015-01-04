namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    public class FooBarImplementation : IFooBar
    {

        public string GetData()
        {
            return "Foo Bar has only one implementation";
        }
    }
}