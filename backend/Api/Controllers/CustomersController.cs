using CustomerRegistrationOnboarding.Application.DTOs;
using CustomerRegistrationOnboarding.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRegistrationOnboarding.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.CreateCustomerAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, cancellationToken);
        return Ok(customer);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllCustomersAsync(cancellationToken);
        return Ok(customers);
    }
}
