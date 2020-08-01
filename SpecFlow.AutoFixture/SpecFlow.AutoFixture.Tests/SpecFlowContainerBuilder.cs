using Autofac;
using Autofac.Features.ResolveAnything;
using SpecFlow.Autofac;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace SpecFlow.AutoFixture.Tests
{
    public class SpecFlowContainerBuilder
    {
        [ScenarioDependencies]
        public static ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();
            var stepDefinitions = typeof(SpecFlowContainerBuilder).Assembly
                .GetTypes().Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))).ToArray();
            builder.RegisterTypes(stepDefinitions).SingleInstance();

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(type => !type.IsModelOrEntity()));

            // For Models and Entities let AutoFixture generate the object so we have the required data populated for tests
            builder.RegisterSource(new DomainDataAutoPopulatedSource());

            return builder;
        }
    }
}
