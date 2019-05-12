using Cleaners.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Account
{
    public class AccountLockoutModel
    {
        [HiddenInput]
        public int UserId { get; set; }

        public bool LockoutDisabled { get; set; }

        [Display(Name = ResourceKeys.LockoutEnd)]
        public DateTime? LockoutEndUtc { get; set; }
    }
}