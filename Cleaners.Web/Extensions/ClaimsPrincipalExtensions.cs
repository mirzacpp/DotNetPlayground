﻿using Cleaners.Web.Infrastructure.Authentication;
using System.Security.Claims;

namespace Cleaners.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        #region Methods

        /// <summary>
        /// Shorthand method for User.isInRole(RoleNames.Admin)
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static bool IsAdmin(this ClaimsPrincipal claims)
        {
            return claims.IsInRole(RoleNames.Admin);
        }

        /// <summary>
        /// Shorthand method for User.isInRole(RoleNames.Support)
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static bool IsSupport(this ClaimsPrincipal claims)
        {
            return claims.IsInRole(RoleNames.Support);
        }

        #endregion Methods
    }
}