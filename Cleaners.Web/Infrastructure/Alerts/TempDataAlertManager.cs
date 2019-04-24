using Cleaners.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace Cleaners.Web.Infrastructure.Alerts
{
    /// <summary>
    /// Stores alerts in tempdata dictionary
    /// </summary>
    public class TempDataAlertManager : IAlertManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public TempDataAlertManager(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _tempDataDictionaryFactory = tempDataDictionaryFactory ?? throw new ArgumentNullException(nameof(tempDataDictionaryFactory));
        }

        public AlertList Alerts
        {
            get
            {
                var tempDataDictionary = _tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);
                var alerts = tempDataDictionary.Get<AlertList>("notification.key");

                //return alerts == null ? AlertList.Empty : alerts;

                return alerts ?? AlertList.Empty;
            }
        }
    }
}