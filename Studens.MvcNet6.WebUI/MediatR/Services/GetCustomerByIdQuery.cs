using Studens.MediatR;

namespace Studens.MvcNet6.WebUI.MediatR.Services
{
	public class GetCustomerByIdQuery : PagedQuery<IList<CustomerDto>>
	{
	}
}