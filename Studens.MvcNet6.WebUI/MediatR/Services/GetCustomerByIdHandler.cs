using MediatR;

namespace Studens.MvcNet6.WebUI.MediatR.Services
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, IList<CustomerDto>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerByIdHandler(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        public async Task<IList<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomer(string.Empty);
        }
    }
}