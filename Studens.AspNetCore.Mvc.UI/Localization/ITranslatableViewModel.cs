namespace Studens.AspNetCore.Mvc.UI.Localization
{
	public interface ITranslatableViewModel
	{
	}

	public class TranslationModel
	{
		public TranslationModel()
		{
			Translations = new List<TranslationEntryModel>();
		}

		public IList<TranslationEntryModel> Translations { get; set; }
	}

	public class TranslationEntryModel
	{
		public string LangCode { get; set; }
		public string Value { get; set; }
	}
}