using CustomerRegistrationOnboarding.Application.DTOs;
using CustomerRegistrationOnboarding.Application.Validation;

namespace CustomerRegistrationOnboarding.Tests;

public class CreateCustomerRequestValidatorTests
{
    private readonly CreateCustomerRequestValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_FirstNameIsEmpty()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "",
            LastName = "Caliboso",
            Email = "carlo.caliboso@email.com",
            PhoneNumber = "09171234567"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCustomerRequest.FirstName));
    }

    [Fact]
    public void Should_HaveError_When_EmailIsInvalid()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "Carlo",
            LastName = "Caliboso",
            Email = "not-an-email",
            PhoneNumber = "09171234567"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCustomerRequest.Email));
    }

    [Fact]
    public void Should_HaveError_When_PhoneNumberIsTooShort()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "Carlo",
            LastName = "Caliboso",
            Email = "carlo.caliboso@email.com",
            PhoneNumber = "123"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCustomerRequest.PhoneNumber));
    }

    [Fact]
    public void Should_NotHaveErrors_When_RequestIsValid()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "Carlo",
            LastName = "Caliboso",
            Email = "carlo.caliboso@email.com",
            PhoneNumber = "+63 917 123 4567"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
