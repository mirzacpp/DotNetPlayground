namespace Simplicity.Commons.Collections
{
	/// <summary>
	/// Predefines <see cref="ITypeList{TBaseType}"/> with object as a base type.
	/// </summary>
	public interface ITypeList : ITypeList<object>
	{
	}

	/// <summary>
	/// Extends <see cref="IList{T}"/> with restriction for a specific base type.
	/// Credits to https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Core/Volo/Abp/Collections/ITypeList.cs
	/// </summary>
	/// <typeparam name="TBaseType">Base type of <see cref="Type"/>s in this list.</typeparam>
	public interface ITypeList<in TBaseType> : IList<Type>
	{
		/// <summary>
		/// Adds a type to list.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		void Add<T>() where T : TBaseType;

		/// <summary>
		/// Adds a type to list if it's not already in the list.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		bool TryAdd<T>() where T : TBaseType;

		/// <summary>
		/// Checks if a type exists in the list.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <returns></returns>
		bool Contains<T>() where T : TBaseType;

		/// <summary>
		/// Removes a type from list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void Remove<T>() where T : TBaseType;
	}
}