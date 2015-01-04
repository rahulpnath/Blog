namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    public class FooGenericImplementation<T> : IFooGeneric<T>
    {

        public string GetData(T dataType)
        {
            return dataType.GetType().Name;
        }
    }
}