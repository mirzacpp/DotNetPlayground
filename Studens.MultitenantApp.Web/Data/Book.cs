using Rev.AuthPermissions.BaseCode.CommonCode;

namespace Studens.MultitenantApp.Web.Data
{
	public class Book : IDataKeyFilterReadWrite
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string UserId { get; set; }
		public string DataKey { get; set; }
	}
}