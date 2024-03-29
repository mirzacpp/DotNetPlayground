﻿using Newtonsoft.Json;

namespace Simplicity.AspNetCore.Mvc.UI.Alerts
{
	/// <summary>
	/// Extension methods for TempData dictionary
	/// </summary>
	public static class TempDataDictionaryExtensions
	{
		/// <summary>
		/// Method used to store complex data inside TempData dictionary, which is not supported by default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tempData"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void InsertSerialized<T>(this ITempDataDictionary tempData, string key, T value) where T : class
		{
			tempData[key] = JsonConvert.SerializeObject(value);
		}

		/// <summary>
		/// Retrieves value from TempData dictonary
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tempData"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T GetDeserialized<T>(this ITempDataDictionary tempData, string key) where T : class
		{
			tempData.TryGetValue(key, out object value);

			return value == null ? null : JsonConvert.DeserializeObject<T>((string)value);
		}
	}
}