using System.Reflection;

namespace Studens.Data.Seed
{
	public static class DataSeedContributorTypeExtensions
	{
		public static IEnumerable<Type> Active(this IEnumerable<Type> types)
		{
			return types.Where(t =>
			{
				var attr = t.GetCustomAttributes<DataSeedAttribute>(true).FirstOrDefault();
				return !attr?.Ignore ?? true;
			});
		}

		public static IEnumerable<Type> Ordered(this IEnumerable<Type> types)
		{
			return types.OrderBy(t =>
			{
				var attr = t.GetCustomAttributes<DataSeedAttribute>(true).FirstOrDefault();
				return attr?.Order ?? 0;
			});
		}

		public static IEnumerable<Type> WithEnvironment(this IEnumerable<Type> types, string? environment)
		{
			return types.Where(t =>
			{
				var attr = t.GetCustomAttributes<DataSeedAttribute>(true).FirstOrDefault();
				return string.IsNullOrEmpty(attr?.Environment) || attr.Environment.Equals(environment, StringComparison.OrdinalIgnoreCase);
			});
		}
	}
}