using CustomerRegistrationOnboarding.Application.DTOs;

namespace CustomerRegistrationOnboarding.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<CustomerResponse> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CustomerResponse>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
}
