using Cleaners.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Web.Infrastructure.Alerts
{
    /// <summary>
    /// Stores alerts in tempdata dictionary
    /// </summary>
    public class TempDataAlertManager
    {
        #region Constants

        /// <summary>
        /// This value is only used inside this class
        /// </summary>
        private const string _tempDataKey = ".notification.key";

        #endregion Constants

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        /// <summary>
        /// Get instance of current dictionary by temp data dictionary factory
        /// </summary>
        private readonly ITempDataDictionary _tempDataDictionary;

        #endregion Fields

        #region Constructor

        public TempDataAlertManager(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _tempDataDictionaryFactory = tempDataDictionaryFactory ?? throw new ArgumentNullException(nameof(tempDataDictionaryFactory));
            _tempDataDictionary = _tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);
        }

        #endregion Constructor

        #region Methods

        public void Success(string message, string title = "", bool isDismissable = true)
        {
            Insert(AlertType.Success, message, title, isDismissable);
        }

        public void Warning(string message, string title = "", bool isDismissable = true)
        {
            Insert(AlertType.Warning, message, title, isDismissable);
        }

        public void Error(string message, string title = "", bool isDismissable = true)
        {
            Insert(AlertType.Danger, message, title, isDismissable);
        }

        public void Info(string message, string title = "", bool isDismissable = true)
        {
            Insert(AlertType.Info, message, title, isDismissable);
        }

        private void Insert(AlertType alertType, string message, string title = "", bool isDismissable = true)
        {
            var alerts = _tempDataDictionary.GetDeserialized<List<AlertItem>>(_tempDataKey) ?? new List<AlertItem>();

            alerts.Add(new AlertItem(alertType, message, title, isDismissable));

            _tempDataDictionary.InsertSerialized(_tempDataKey, alerts);
        }

        public List<AlertItem> Get(AlertType alertType)
        {
            var alerts = _tempDataDictionary.GetDeserialized<List<AlertItem>>(_tempDataKey) ?? new List<AlertItem>();

            return alerts.Where(a => a.Type == alertType).ToList();
        }

        public List<AlertItem> GetAll()
        {
            return _tempDataDictionary.GetDeserialized<List<AlertItem>>(_tempDataKey) ?? new List<AlertItem>();
        }

        #endregion Methods
    }
}