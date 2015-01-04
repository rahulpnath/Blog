namespace IocConventionRegistration.Implementations
{
    using IocConventionRegistration.Interfaces;

    using Microsoft.Practices.Unity;

    public class UnityDependencyResolver : IDependencyResolver   
    {
        private readonly IUnityContainer unityContainer;

        public UnityDependencyResolver(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public T Resolve<T>()
        {
            return this.unityContainer.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return this.unityContainer.Resolve<T>(name);
        }
    }
}