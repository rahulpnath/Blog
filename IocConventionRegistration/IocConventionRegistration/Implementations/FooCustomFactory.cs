namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    public class FooCustomFactory : IFooCustomFactory   
    {
        private readonly IDependencyResolver dependencyResolver;

        public FooCustomFactory(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public IFooCustom Create(string customContext)
        {
            return this.dependencyResolver.Resolve<IFooCustom>(customContext);
        }
    }
}