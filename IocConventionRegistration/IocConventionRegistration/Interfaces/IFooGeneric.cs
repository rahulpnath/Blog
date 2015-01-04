namespace IocConventionRegistration.Interfaces
{
    public interface IFooGeneric<T>
    {
        string GetData(T dataType);
    }
}