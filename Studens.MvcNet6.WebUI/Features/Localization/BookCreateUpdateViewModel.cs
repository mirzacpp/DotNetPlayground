﻿using Studens.AspNetCore.Mvc.UI.Localization;

namespace Studens.MvcNet6.WebUI.Features.Localization
{
	public class BookCreateUpdateViewModel : ITranslatableViewModel
	{
		public BookCreateUpdateViewModel()
		{
			Title = new();
			Description = new();
		}

		/// <summary>
		/// Contains translations for Title
		/// </summary>
		public TranslationModel Title { get; set; }

		public TranslationModel Description { get; set; }
	}
}