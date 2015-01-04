namespace IocConventionRegistration.Interfaces
{
    public interface IFooFactory
    {
        IFoo Create(string fooKey);
    }
}