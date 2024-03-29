﻿using System.Globalization;

namespace Simplicity.Commons.Localization
{
	public static class LanguageInfoExtensions
	{
		public static bool IsRtl(this ILanguageInfo languageInfo)
		{
			return new CultureInfo(languageInfo.CultureName).TextInfo.IsRightToLeft;
		}
	}
}