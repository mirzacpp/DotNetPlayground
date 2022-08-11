namespace Studens.Commons.Localization
{
	public interface ILanguageInfo
	{
		string CultureName { get; }
		string UiCultureName { get; }
		string DisplayName { get; }
		bool IsRtl { get; }
		string? FlagIcon { get; }
	}
}