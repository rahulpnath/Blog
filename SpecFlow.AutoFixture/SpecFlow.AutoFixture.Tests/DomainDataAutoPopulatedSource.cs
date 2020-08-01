using Autofac.Builder;
using Autofac.Core;
using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecFlow.AutoFixture.Tests
{
    public class DomainDataAutoPopulatedSource : IRegistrationSource
    {
        public bool IsAdapterForIndividualComponents => false;

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {

            var swt = service as IServiceWithType;

            if (swt == null || !swt.ServiceType.IsModelOrEntity())
                return Enumerable.Empty<IComponentRegistration>();

            object instance = null;
            try
            {

                instance = new SpecimenContext(GetFixture()).Resolve(swt.ServiceType);
            }
            catch (Exception)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            return new[] { RegistrationBuilder.ForDelegate(swt.ServiceType, (c, p) => instance).CreateRegistration() };
        }

        public static bool IsModelOrEntity(Type type)
        {
            return type.IsModelOrEntity() || type.IsGenericModelOrEntity();
        }

        private static Fixture GetFixture()
        {
            var fixture = new Fixture();

            return fixture;
        }
    }
}
