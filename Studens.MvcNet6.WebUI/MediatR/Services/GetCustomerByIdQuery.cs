using Simplicity.MediatR;

namespace Simplicity.MvcNet6.WebUI.MediatR.Services
{
	public class GetCustomerByIdQuery : PagedQuery<IList<CustomerDto>>
	{
	}
}