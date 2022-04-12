namespace Studens.MvcNet6.WebUI.MediatR.Services
{
    public interface ICustomerService
    {
        Task<IList<CustomerDto>> GetCustomer(string customerId);

        Task AddCustomerAsync(CustomerDto customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IList<CustomerDto> _customerList;

        public CustomerService()
        {
            _customerList = Enumerable.Range(1, 5)
                .Select(i => new CustomerDto(Guid.NewGuid().ToString(), $"user{i}@mail.mail"))
                .ToList();
        }

        public Task AddCustomerAsync(CustomerDto customer)
        {
            if (!_customerList.Contains(customer))
            {
                _customerList.Add(customer);
            }

            return Task.CompletedTask;
        }

        public Task<IList<CustomerDto>> GetCustomer(string customerId) =>
            Task.FromResult(_customerList);
    }

    public record CustomerDto
    {
        public CustomerDto(string id, string email)
        {
            Id = id;
            Email = email;
        }

        public string Id { get; set; }

        public string Email { get; set; }
    }
}