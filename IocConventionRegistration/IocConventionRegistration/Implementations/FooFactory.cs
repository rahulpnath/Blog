namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    public class FooFactory : IFooFactory
    {
        private readonly IDependencyResolver dependencyResolver;

        public FooFactory(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public IFoo Create(string fooKey)
        {
            var fooConvention = string.Format("FooImplementation{0}", fooKey);
            return this.dependencyResolver.Resolve<IFoo>(fooConvention);
        }
    }
}