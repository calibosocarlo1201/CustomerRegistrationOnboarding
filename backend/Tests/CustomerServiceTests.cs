using CustomerRegistrationOnboarding.Application.DTOs;
using CustomerRegistrationOnboarding.Application.Exceptions;
using CustomerRegistrationOnboarding.Application.Services;
using CustomerRegistrationOnboarding.Application.Validation;
using CustomerRegistrationOnboarding.Domain.Entities;
using CustomerRegistrationOnboarding.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerRegistrationOnboarding.Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly IValidator<CreateCustomerRequest> _validator;
    private readonly Mock<ILogger<CustomerService>> _loggerMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _validator = new CreateCustomerRequestValidator();
        _loggerMock = new Mock<ILogger<CustomerService>>();
        _service = new CustomerService(_repositoryMock.Object, _validator, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidRequest_ReturnsCustomerResponse()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "Carlo",
            LastName = "Caliboso",
            Email = "carlo.caliboso@email.com",
            PhoneNumber = "+63 917 123 4567",
            SignatureBase64 = "data:image/png;base64,abc123"
        };

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer c, CancellationToken _) => c);

        var result = await _service.CreateCustomerAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Carlo", result.FirstName);
        Assert.Equal("Caliboso", result.LastName);
        Assert.Equal("carlo.caliboso@email.com", result.Email);
        Assert.Equal("+63 917 123 4567", result.PhoneNumber);
        Assert.True(result.DateCreated <= DateTime.UtcNow);

        _repositoryMock.Verify(
            r => r.AddAsync(It.Is<Customer>(c =>
                c.FirstName == "Carlo" &&
                c.Email == "carlo.caliboso@email.com"), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithDuplicateEmail_ThrowsValidationException()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "Carlo",
            LastName = "Caliboso",
            Email = "carlo.caliboso@email.com",
            PhoneNumber = "+63 917 123 4567"
        };

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<AppValidationException>(
            () => _service.CreateCustomerAsync(request));

        Assert.True(exception.Errors.ContainsKey("Email"));
        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.GetCustomerByIdAsync(id));
    }
}
