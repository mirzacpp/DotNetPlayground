// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rev.AuthPermissions.BaseCode.SetupCode;
using Rev.AuthPermissions.SetupCode;
using StatusGeneric;

namespace Rev.AuthPermissions.BulkLoadServices
{

    /// <summary>
    /// Bulk load many Roles with their permissions
    /// </summary>
    public interface IBulkLoadRolesService
    {
        /// <summary>
        /// This allows you to add Roles with their permissions via the <see cref="BulkLoadRolesDto"/> class
        /// </summary>
        /// <param name="roleSetupData">A list of definitions containing the information for each Role</param>
        /// <returns>status</returns>
        Task<IStatusGeneric> AddRolesToDatabaseAsync(List<BulkLoadRolesDto> roleSetupData);
    }
}