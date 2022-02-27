using System.ComponentModel;
using System.Globalization;

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
    /// Just a nicer way for object casting
    /// </summary>
    public static T As<T>(this object obj) where T : class
        => (T)obj;

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
    /// Converts given object to a specified type
    /// </summary>
    public static T? To<T>(this object obj) => (T?)To(obj, typeof(T));

    /// <summary>
    /// Converts given object to a specified type
    /// </summary>
    public static object? To(this object value, Type destinationType) => To(value, destinationType, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts given object to a specified type
    /// </summary>
    public static object? To(this object value, Type destinationType, CultureInfo culture)
    {
        if (value == null)
            return null;

        var sourceType = value.GetType();

        var destinationConverter = TypeDescriptor.GetConverter(destinationType);
        if (destinationConverter.CanConvertFrom(value.GetType()))
            return destinationConverter.ConvertFrom(null, culture, value);

        var sourceConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter.CanConvertTo(destinationType))
            return sourceConverter.ConvertTo(null, culture, value, destinationType);

        if (destinationType.IsEnum && value is int)
            return Enum.ToObject(destinationType, (int)value);

        if (!destinationType.IsInstanceOfType(value))
            return Convert.ChangeType(value, destinationType, culture);

        return value;
    }

    /// <summary>
    /// Syntatic sugar for creating collection out of single item
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="obj">Object</param>
    /// <returns>Collection</returns>
    public static IEnumerable<T> AsCollection<T>(this T obj) => new[] { obj };

    public static IList<T> AsList<T>(this T obj) => new[] { obj };
}