using MediatR;

namespace Studens.MvcNet6.WebUI.MediatR.Services
{
    public class GetCustomerByIdQuery : IRequest<IList<CustomerDto>>
    {
    }
}