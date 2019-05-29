﻿using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Models.Roles
{
    public class RoleUpdateModel : RoleCreateModel
    {
        [HiddenInput]
        public int Id { get; set; }
    }
}