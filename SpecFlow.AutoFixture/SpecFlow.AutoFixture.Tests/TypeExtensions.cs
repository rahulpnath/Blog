using SpecFlow.AutoFixture.Web.Models;
using System;
using System.Linq;

namespace SpecFlow.AutoFixture.Tests
{
    public static class TypeExtensions
    {
        public static bool IsModelOrEntity(this Type type)
        {
            if (type == null) return false;

            return type.Namespace == typeof(Customer).Namespace;
        }

        public static bool IsGenericModelOrEntity(this Type type)
        {
            if (type == null) return false;

            return type.IsGenericType && type.GenericTypeArguments.Any(a => a.IsModelOrEntity());
        }
    }
}
