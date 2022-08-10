namespace Studens.Commons.Localization
{
	public interface ILanguageProvider
	{
		Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync();
	}
}