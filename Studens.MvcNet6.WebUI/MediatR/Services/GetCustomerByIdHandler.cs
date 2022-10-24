using MediatR;

namespace Simplicity.MvcNet6.WebUI.MediatR.Services
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
            return await _customerService.GetCustomer(page: 3, pageSize: 1);
        }
    }
}