namespace Studens.Commons.DependencyInjection;

/// <summary>
/// Classes marked with this attribute will be registered as scoped dependencies
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ScopeDependencyAttribute : Attribute
{
}