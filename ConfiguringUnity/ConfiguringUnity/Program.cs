using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguringUnity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var unitySample = new UnitySample();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }
    }

    class UnitySample
    {
        private IUnityContainer unityContainer;

        public UnitySample()
        {
            this.unityContainer = new UnityContainer();
            this.RegisterANormalInterface();
            this.RegisterAClass();
            this.RegisterGeniericInterface();
            this.registerGenericInterfaceWithCompexGenericTypes();
            this.RegisterConflictingInterfaces();
            this.RegisterOverridableDependency();
            
            // Load from configuration
            this.unityContainer.LoadConfiguration();

            this.CheckTypesResolution();

        }

        private void RegisterOverridableDependency()
        {
            this.unityContainer.RegisterType<IOverridableDependency, OverridableCodeImplementation>();
        }

        private void RegisterConflictingInterfaces()
        {
            this.unityContainer.RegisterType<IConflictingInterface, ConflictingInterfaceImplementation>();
            this.unityContainer.RegisterType<ExternalLibrary.IConflictingInterface, ExternalLibrary.ConflictingInterfaceImplementation>();
        }

        private void registerGenericInterfaceWithCompexGenericTypes()
        {
            this.unityContainer.RegisterType<IComplexGenericInterface<ComplexGenericClass<GenericClass>>, ComplexGenericInterfaceImplementation>();
        }

        private void CheckTypesResolution()
        {
            var normalInterface = this.unityContainer.Resolve<INormalInterface>();
            Debug.Assert(normalInterface != null);

            var normalClass = this.unityContainer.Resolve<NormalClass>();
            Debug.Assert(normalClass != null);

            var genericInterface = this.unityContainer.Resolve<IGenericInterface<GenericClass>>();
            Debug.Assert(genericInterface != null);

            var genericInterfaceWithTwoParameters = this.unityContainer.Resolve<IGenericInterfaceWithTwoParameter<GenericClass, AnotherGenericClass>>();
            Debug.Assert(genericInterfaceWithTwoParameters != null);

            var complexGenericInterface =
                this.unityContainer.Resolve<IComplexGenericInterface<ComplexGenericClass<GenericClass>>>();
            Debug.Assert(complexGenericInterface != null);

            var conflictingInterface1 = this.unityContainer.Resolve<IConflictingInterface>();
            Debug.Assert(conflictingInterface1 != null);

            var conflictingInterface2 = this.unityContainer.Resolve<ExternalLibrary.IConflictingInterface>();
            Debug.Assert(conflictingInterface2 != null);

            var overridableDependency = this.unityContainer.Resolve<IOverridableDependency>();
            Debug.Assert(overridableDependency != null);
            Debug.Assert(overridableDependency.GetType() == typeof(OverridableConfigImplementation));
        }

        private void RegisterGeniericInterface()
        {
            this.unityContainer.RegisterType(typeof(IGenericInterface<>), typeof(GenericInterfaceImplementation<>));
            this.unityContainer.RegisterType(typeof(IGenericInterfaceWithTwoParameter<,>), typeof(GenericInterfaceWithTwoParametersImplementation<,>));
        }

        private void RegisterAClass()
        {
            this.unityContainer.RegisterType<NormalClass>();
        }

        public void RegisterANormalInterface()
        {
            this.unityContainer.RegisterType<INormalInterface, NormalInterfaceImplementation>();
        }
    }
}
