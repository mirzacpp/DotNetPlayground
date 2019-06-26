﻿using Cleaners.Web.Constants;
using Cleaners.Web.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cleaners.Web.Controllers
{
    [Route("localization")]
    public class LocalizationController : CorvoControllerBase
    {
        #region Methods

        /// <summary>
        /// Marks selected culture as active
        /// </summary>
        /// <param name="culture">Selected culture</param>
        /// <returns>Previous or home page</returns>
        [Route("{culture}", Name = LocalizationRoutes.ChangeLanguage)]
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(
                $"{ CookieDefaults.Prefix}{ CookieDefaults.CultureCookie}",
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                // Move this to constants
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToPreviousUrl();
        }

        #endregion Methods
    }
}