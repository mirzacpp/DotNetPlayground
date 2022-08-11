using Microsoft.Extensions.Options;

namespace Studens.Commons.Localization
{
	public class DefaultLanguageProvider : ILanguageProvider
	{
		private readonly LocalizationOptions _options;

		public DefaultLanguageProvider(IOptions<LocalizationOptions> optionsAccessor)
		{
			_options = optionsAccessor.Value;
		}

		public Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync() =>
		Task.FromResult((IReadOnlyList<LanguageInfo>)_options.Languages);
	}
}