using Rev.AuthPermissions.BaseCode.CommonCode;
using Studens.Domain.Entities;

namespace Studens.MultitenantApp.Web.Data
{
	public class TeamMember : IEntity<string>, IDataKeyFilterReadWrite
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DataKey { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}