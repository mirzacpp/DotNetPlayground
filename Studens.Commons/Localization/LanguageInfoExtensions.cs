using System.Globalization;

namespace Studens.Commons.Localization
{
	public static class LanguageInfoExtensions
	{
		public static bool IsRtl(this ILanguageInfo languageInfo)
		{
			return new CultureInfo(languageInfo.CultureName).TextInfo.IsRightToLeft;
		}
	}
}