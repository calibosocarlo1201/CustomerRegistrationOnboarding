using CustomerRegistrationOnboarding.Application.DTOs;
using CustomerRegistrationOnboarding.Application.Exceptions;
using CustomerRegistrationOnboarding.Application.Interfaces;
using CustomerRegistrationOnboarding.Domain.Entities;
using CustomerRegistrationOnboarding.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CustomerRegistrationOnboarding.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IValidator<CreateCustomerRequest> _validator;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository repository,
        IValidator<CreateCustomerRequest> validator,
        ILogger<CustomerService> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<CustomerResponse> CreateCustomerAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            throw new AppValidationException(errors);
        }

        if (await _repository.EmailExistsAsync(request.Email, cancellationToken))
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["Email"] = ["A customer with this email already exists."]
            });
        }

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PhoneNumber = request.PhoneNumber.Trim(),
            SignatureBase64 = request.SignatureBase64,
            DateCreated = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(customer, cancellationToken);
        _logger.LogInformation("Customer created with ID {CustomerId}", created.Id);

        return MapToResponse(created);
    }

    public async Task<CustomerResponse> GetCustomerByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            throw new NotFoundException($"Customer with ID '{id}' was not found.");
        }

        return MapToResponse(customer);
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllCustomersAsync(
        CancellationToken cancellationToken = default)
    {
        var customers = await _repository.GetAllAsync(cancellationToken);
        return customers.Select(MapToResponse).ToList();
    }

    private static CustomerResponse MapToResponse(Customer customer) => new()
    {
        Id = customer.Id,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        Email = customer.Email,
        PhoneNumber = customer.PhoneNumber,
        DateCreated = customer.DateCreated
    };
}
