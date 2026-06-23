using CustomerRegistrationOnboarding.Domain.Entities;

namespace CustomerRegistrationOnboarding.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
