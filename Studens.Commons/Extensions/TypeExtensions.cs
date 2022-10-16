namespace System;

public static class TypeExtensions
{
	/// <summary>
	/// Determines if <paramref name="type"/> is open/unbound generic type.
	/// </summary>
	public static bool IsOpenGenericType(this Type type) =>
		type.IsGenericTypeDefinition || type.ContainsGenericParameters;

	/// <summary>
	/// Returns assemblies for given <paramref name="types"/>.
	/// </summary>
	public static IEnumerable<Assembly> GetAssembliesFromTypes(this IEnumerable<Type> types) =>
		types.Select(t => t.GetTypeInfo().Assembly);

	/// <summary>
	/// Determines if <paramref name="type"/> is attribute type.
	/// </summary>
	public static bool IsAttribute(this Type type) => type.IsSubclassOf(typeof(Attribute));

	public static bool HasMatchingGenericArity(this Type type1, Type type2)
	{
		if (type1.IsGenericType)
		{
			if (type2.IsGenericType)
			{
				var argumentCount = type1.GetGenericArguments().Length;
				var parameterCount = type2.GetGenericArguments().Length;

				return argumentCount == parameterCount;
			}

			return false;
		}

		return true;
	}
}