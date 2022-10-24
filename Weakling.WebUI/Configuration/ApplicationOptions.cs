namespace Simplicity.WebUI.Configuration;

/// <summary>
/// Defines application options
/// </summary>
public class ApplicationOptions
{
	public SetupOptions Setup { get; set; } = new SetupOptions();
}

public class SetupOptions
{
	/// <summary>
	/// Indicates whether http loging should be enabled.
	/// </summary>
	public bool EnableHttpLogging { get; set; } = false;

	/// <summary>
	/// Indicates whether miniprofiler should be enabled.
	/// </summary>
	public bool EnableMiniProfiler { get; set; } = false;

	/// <summary>
	/// Indicates whether host filtering should be enabled.
	/// </summary>
	public bool EnableHostFiltering { get; set; } = false;

	/// <summary>
	/// Indicates whether https redirection should be enabled.
	/// </summary>
	public bool EnableHttpsRedirection { get; set; } = false;
}