namespace IocConventionRegistration.Interfaces
{
    public interface IFooCustomFactory
    {
        IFooCustom Create(string customContext);
    }
}