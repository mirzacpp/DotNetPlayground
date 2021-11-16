namespace Studens.Commons.DependencyInjection;

/// <summary>
/// Classes marked with this attribute will be registered as transient dependencies
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class TransientDependencyAttribute : Attribute
{
}
