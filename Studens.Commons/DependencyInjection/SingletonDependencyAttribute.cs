namespace Studens.Commons.DependencyInjection;
/// <summary>
/// Classes marked with this attribute will be registered as singleton dependencies
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SingletonDependencyAttribute : Attribute
{
}
