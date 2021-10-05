using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns generic type definition if <paramref name="type"/> is generic type or defaults to <paramref name="type"/>
        /// </summary>
        public static Type GetGenericTypeDefinitionOrDefault(this Type type) =>
            type.IsOpenGenericType() ? type.GetGenericTypeDefinition() : type;

        /// <summary>
        /// Determines if <paramref name="type"/> is open/unbound generic type
        /// </summary>
        public static bool IsOpenGenericType(this Type type) =>
            type.IsGenericTypeDefinition || type.ContainsGenericParameters;

        /// <summary>
        /// Returns assemblies for given <paramref name="types"/>
        /// </summary>
        public static IEnumerable<Assembly> GetAssembliesFromTypes(this IEnumerable<Type> types) =>
            types.Select(t => t.GetTypeInfo().Assembly);
    }
}