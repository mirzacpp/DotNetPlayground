// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.SetupCode;

namespace Rev.AuthPermissions.AdminCode;

/// <summary>
/// Methods to decode the <see cref="AuthPermissionsOptions.TenantType"/> property
/// </summary>
public static class TenantTypeExtensions
{
	/// <summary>
	/// This checks that the <see cref="AuthPermissionsOptions.TenantType"/> property contains a valid state
	/// </summary>
	/// <param name="tenantType"></param>
	/// <exception cref="AuthPermissionsException"></exception>
	public static void ThrowExceptionIfTenantTypeIsWrong(this TenantTypes tenantType)
	{
		if (!tenantType.IsMultiTenant() && tenantType.HasFlag(TenantTypes.AddSharding))
			throw new AuthPermissionsException(
				$"You need to set the {nameof(AuthPermissionsOptions.TenantType)} option to {nameof(TenantTypes.SingleLevel)} when setting the {nameof(TenantTypes.AddSharding)} flag.");
	}

	/// <summary>
	/// Returns true if the <see cref="AuthPermissionsOptions.TenantType"/> property is set to use AuthP's multi-tenant feature
	/// </summary>
	/// <param name="tenantType"></param>
	/// <returns></returns>
	public static bool IsMultiTenant(this TenantTypes tenantType)
	{
		return tenantType.HasFlag(TenantTypes.SingleLevel);
	}

	/// <summary>
	/// Returns true if the <see cref="AuthPermissionsOptions.TenantType"/> property is set to <see cref="TenantTypes.SingleLevel"/>
	/// </summary>
	/// <param name="tenantType"></param>
	public static bool IsSingleLevel(this TenantTypes tenantType)
	{
		return tenantType.HasFlag(TenantTypes.SingleLevel);
	}

	/// <summary>
	/// Returns true if the <see cref="AuthPermissionsOptions.TenantType"/> property has the <see cref="TenantTypes.AddSharding"/> flag set
	/// </summary>
	/// <param name="tenantType"></param>
	public static bool IsSharding(this TenantTypes tenantType)
	{
		return tenantType.HasFlag(TenantTypes.AddSharding);
	}
}