using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq;

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

                if (!tempDataDictionary.Keys.Any())
                {
                    return AlertList.Empty;
                }

                var list = (AlertList)tempDataDictionary["notification.key"];

                return list;
            }
        }
    }
}