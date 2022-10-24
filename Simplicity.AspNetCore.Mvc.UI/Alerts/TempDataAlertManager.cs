using Microsoft.AspNetCore.Http;

namespace Simplicity.AspNetCore.Mvc.UI.Alerts
{
	/// <summary>
	/// Alert manager implementation using <see cref="Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary"/> dictionary
	/// </summary>
	public class TempDataAlertManager : IAlertManager
	{
		private const string TempDataKey = ".alerts.key";

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

		/// <summary>
		/// Get instance of current dictionary by temp data dictionary factory
		/// </summary>
		private readonly ITempDataDictionary _tempDataDictionary;

		public TempDataAlertManager(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
		{
			_httpContextAccessor = httpContextAccessor;
			_tempDataDictionaryFactory = tempDataDictionaryFactory;
			_tempDataDictionary = tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);
		}

		private List<AlertItem> AlertItems
		{
			get
			{
				return _tempDataDictionary.GetDeserialized<List<AlertItem>>(TempDataKey) ?? new List<AlertItem>();
			}
		}

		public void Add(AlertType alertType, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException($"{text} cannot be null or empty.", nameof(text));
			}

			AddToDictionary(new AlertItem(alertType, text));
		}

		public void Add(AlertType alertType, string text, string title)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException($"{text} cannot be null or empty.", nameof(text));
			}

			if (string.IsNullOrEmpty(title))
			{
				throw new ArgumentException($"{title} cannot be null or empty.", nameof(title));
			}

			AddToDictionary(new AlertItem(alertType, text, title));
		}

		public void Add(AlertType alertType, string text, string title, bool isDismissable)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException($"{text} cannot be null or empty.", nameof(text));
			}

			if (string.IsNullOrEmpty(title))
			{
				throw new ArgumentException($"{title} cannot be null or empty.", nameof(title));
			}

			AddToDictionary(new AlertItem(alertType, text, title, isDismissable));
		}

		public void Success(string text)
		{
			Add(AlertType.Success, text);
		}

		public void Success(string text, string title)
		{
			Add(AlertType.Success, text, title);
		}

		public void Success(string text, string title, bool isDismissable)
		{
			Add(AlertType.Success, text, title, isDismissable);
		}

		public void Info(string text)
		{
			Add(AlertType.Info, text);
		}

		public void Info(string text, string title)
		{
			Add(AlertType.Info, text, title);
		}

		public void Info(string text, string title, bool isDismissable)
		{
			Add(AlertType.Info, text, title, isDismissable);
		}

		public void Add(AlertItem alertItem)
		{
			if (alertItem == null)
			{
				throw new ArgumentNullException(nameof(alertItem));
			}

			AddToDictionary(alertItem);
		}

		public IEnumerable<AlertItem> GetAll()
		{
			return AlertItems;
		}

		public IEnumerable<AlertItem> GetByType(AlertType alertType)
		{
			return AlertItems.Where(a => a.Type == alertType);
		}

		private void AddToDictionary(AlertItem alertItem)
		{
			if (alertItem == null)
			{
				throw new ArgumentNullException(nameof(alertItem));
			}

			AlertItems.Add(alertItem);

			_tempDataDictionary.InsertSerialized(TempDataKey, AlertItems);
		}

		/// <summary>
		/// Allow user to store multiple alerts at the same time, so we avoid multiple serialization/deserialization
		/// </summary>
		/// <param name="alertItems"></param>
		private void AddToDictionary(IEnumerable<AlertItem> alertItems)
		{
			if (alertItems == null)
			{
				throw new ArgumentNullException(nameof(alertItems));
			}

			AlertItems.AddRange(alertItems);

			_tempDataDictionary.InsertSerialized(TempDataKey, AlertItems);
		}
	}
}