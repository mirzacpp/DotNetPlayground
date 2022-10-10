namespace Weakling.WebUI.Configuration;

/// <summary>
/// Defines application options
/// </summary>
public class ApplicationOptions
{
	/// <summary>
	/// Indicates whether http loging should be enabled.
	/// </summary>
	public bool EnableHttpLogging { get; set; } = false;

	/// <summary>
	/// Indicates whether miniprofiler should be enabled.
	/// </summary>
	public bool EnableMiniProfiler { get; set; } = false;
}