namespace IocConventionRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    public class IoCConvention
    {
        private const string ApplicationNamespace = "IocConventionRegistration";

        private static Dictionary<Type, HashSet<Type>> internalTypeMapping = new Dictionary<Type, HashSet<Type>>();

        public static void RegisterByConvention(IUnityContainer container, IEnumerable<Assembly> assembliesToLoad = null)
        {
            foreach (var type in GetClassesFromAssembliesInBasePath(assembliesToLoad))
            {
                var interfacesToBeRegsitered = GetInterfacesToBeRegistered(type);
                AddToInternalTypeMapping(type, interfacesToBeRegsitered);
            }

            RegisterConventions(container);
        }

        private static void RegisterConventions(IUnityContainer container)
        {
            foreach (var typeMapping in internalTypeMapping)
            {
                if (typeMapping.Value.Count == 1)
                {
                    var type = typeMapping.Value.First();
                    container.RegisterType(typeMapping.Key, type);
                }
                else
                {
                    foreach (var type in typeMapping.Value)
                    {
                        container.RegisterType(typeMapping.Key, type, GetNameForRegsitration(type));
                    }
                }
            }
        }

        private static string GetNameForRegsitration(Type type)
        {
            var name = type.Name;
            var iocAttribute = (IoCConventionAttribute)Attribute.GetCustomAttribute(type, typeof(IoCConventionAttribute));
            if (iocAttribute != null)
            {
                name = iocAttribute.ShouldAppendClassName ? iocAttribute.Name + name : iocAttribute.Name;
            }

            return name;
        }

        private static void AddToInternalTypeMapping(Type type, IEnumerable<Type> interfacesOnType)
        {
            foreach (var interfaceOnType in interfacesOnType)
            {
                if (!internalTypeMapping.ContainsKey(interfaceOnType))
                {
                    internalTypeMapping[interfaceOnType] = new HashSet<Type>();
                }

                internalTypeMapping[interfaceOnType].Add(type);
            }
        }


        private static IEnumerable<Type> GetInterfacesToBeRegistered(Type type)
        {
            var allInterfacesOnType = type.GetInterfaces()
                .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i).ToList();

            return allInterfacesOnType.Except(allInterfacesOnType.SelectMany(i => i.GetInterfaces())).ToList();
        }

        private static IEnumerable<Type> GetClassesFromAssembliesInBasePath(IEnumerable<Assembly> assemblies = null)
        {
            var allClasses = assemblies != null ? AllClasses.FromAssemblies(assemblies) : AllClasses.FromAssembliesInBasePath();
            return
                allClasses.Where(
                    n =>
                        n.Namespace != null
                        && n.Namespace.StartsWith(ApplicationNamespace, StringComparison.InvariantCultureIgnoreCase));
        }                                                                                                        

    }
}