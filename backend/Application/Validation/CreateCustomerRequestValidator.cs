using CustomerRegistrationOnboarding.Application.DTOs;
using FluentValidation;

namespace CustomerRegistrationOnboarding.Application.Validation;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^[\d\s\-\+\(\)]+$").WithMessage("Phone number contains invalid characters.")
            .MinimumLength(7).WithMessage("Phone number must be at least 7 characters.")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");
    }
}
