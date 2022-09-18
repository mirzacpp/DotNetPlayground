using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

/// <summary>
/// Do not change namespace
/// </summary>
namespace System;

/// <summary>
/// Extenions methods for all objects
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// Used to simplify and beautify casting an object to a type.
	/// </summary>
	/// <typeparam name="T">Type to be casted</typeparam>
	/// <param name="obj">Object to cast</param>
	/// <returns>Casted object</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T As<T>(this object obj)
		where T : class
	{
		return (T)obj;
	}

	/// <summary>
	/// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
	/// </summary>
	/// <param name="obj">Object to be converted</param>
	/// <typeparam name="T">Type of the target object</typeparam>
	/// <returns>Converted object</returns>
	public static T To<T>(this object obj)
		where T : struct
	{
		if (typeof(T) == typeof(Guid))
		{
			return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
		}

		return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Conditionally performs function on an object and returns the modified or the original object.
	/// Used mostly to replace multiline if-assign cases.
	/// </summary>
	public static T If<T>(this T obj, bool condition, Func<T, T> func)
	{
		if (condition)
		{
			return func(obj);
		}

		return obj;
	}

	/// <summary>
	/// Conditionally performs function on an object and returns the original object.
	/// Used mostly to replace multiline if-assign cases.
	/// </summary>
	public static T If<T>(this T obj, bool condition, Action<T> action)
	{
		if (condition)
		{
			action(obj);
		}

		return obj;
	}

	/// <summary>
	/// Syntatic sugar for creating collection out of single item.
	/// </summary>
	/// <typeparam name="T">Type</typeparam>
	/// <param name="obj">Object</param>
	/// <returns>Collection</returns>
	public static IEnumerable<T> AsCollection<T>(this T obj) => new[] { obj };

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static IList<T> AsList<T>(this T obj) => new[] { obj };
}