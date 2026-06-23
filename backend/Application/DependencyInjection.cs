using CustomerRegistrationOnboarding.Application.DTOs;
using CustomerRegistrationOnboarding.Application.Interfaces;
using CustomerRegistrationOnboarding.Application.Services;
using CustomerRegistrationOnboarding.Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerRegistrationOnboarding.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IValidator<CreateCustomerRequest>, CreateCustomerRequestValidator>();
        return services;
    }
}
