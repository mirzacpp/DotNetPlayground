// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace Rev.AuthPermissions.BaseCode.SetupCode;

/// <summary>
/// Options for the AccessTenantData feature
/// </summary>
public enum LinkToTenantTypes
{
    /// <summary>
    /// Not used
    /// </summary>
    NotTurnedOn,
    /// <summary>
    /// Only app users can do this, i.e. a AuthUser not linked to a tenant
    /// Simpler and quicker than the <see cref="AppAndHierarchicalUsers"/> option
    /// </summary>
    OnlyAppUsers    
}