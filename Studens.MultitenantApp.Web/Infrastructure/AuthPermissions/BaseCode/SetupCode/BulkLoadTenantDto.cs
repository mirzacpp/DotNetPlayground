// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;

namespace Rev.AuthPermissions.BaseCode.SetupCode;

/// <summary>
/// This class is used to bulk loading tenants into the AuthP's database on startup
/// </summary>
public class BulkLoadTenantDto
{
	/// <summary>
	/// This defines a tenant in an multi-tenant application
	/// </summary>
	/// <param name="tenantName">Name of the specific tenant level. So, for hierarchical tenant you only give the name at this tenant level.</param>
	/// <param name="tenantRolesCommaDelimited">Optional: comma delimited string containing the names of tenant Roles for this tenant. NOTE:
	///     - If null in a hierarchical multi-tenant system, then the parent's list of tenant Roles are used
	///     - If empty in a hierarchical multi-tenant system, then it doesn't use the parents list of tenant Role</param>
	public BulkLoadTenantDto(string tenantName,
		string tenantRolesCommaDelimited = null)
	{
		TenantName = tenantName?.Trim() ?? throw new ArgumentNullException(nameof(tenantName));
		TenantRolesCommaDelimited = tenantRolesCommaDelimited;
	}

	/// <summary>
	/// Name of this specific tenant level.
	/// - For single-level tenants its the tenant name
	/// - For hierarchical multi-tenant, its the specific name of the tenant level
	///   e.g. if you adding the shop Dress4U, the TenantName is "Dress4U" and the fullname
	/// </summary>
	public string TenantName { get; }

	/// <summary>
	/// Optional: You can add AuthP's tenant roles via this string
	/// The Roles must have a <see cref="RoleToPermissions.RoleType"/> of <see cref="RoleTypes.TenantAutoAdd"/> or <see cref="RoleTypes.TenantAdminAdd"/>
	/// NOTE:
	/// - If null in a hierarchical multi-tenant system, then the parent's list of tenant Roles are used
	/// - If empty in a hierarchical multi-tenant system, then it doesn't use the parents list of tenant Roles
	/// </summary>
	public string TenantRolesCommaDelimited { get; set; }	

	/// <summary>
	/// Useful for debug
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return $"{nameof(TenantName)}: {TenantName}, {nameof(TenantRolesCommaDelimited)}: {TenantRolesCommaDelimited}";
	}
}