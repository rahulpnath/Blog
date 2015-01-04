namespace IocConventionRegistration.Interfaces
{
    public interface IDependencyResolver
    {
        T Resolve<T>();

        T Resolve<T>(string name);
    }
}