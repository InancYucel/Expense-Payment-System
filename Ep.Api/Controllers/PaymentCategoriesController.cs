using Base.Response;
using Business.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace Expense_Payment_System.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<List<PaymentCategoriesResponse>>> Get()
    {
        var operation = new PaymentCategoriesCqrs.GetAllPaymentCategoriesQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<PaymentCategoriesResponse>> Get(int id)
    {
        var operation = new PaymentCategoriesCqrs.GetPaymentCategoriesByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<PaymentCategoriesResponse>> Post([FromBody] PaymentCategoriesRequest staffRequest)
    {
        var operation = new PaymentCategoriesCqrs.CreatePaymentCategoriesCommand(staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] PaymentCategoriesRequest staffRequest)
    {
        var operation = new PaymentCategoriesCqrs.UpdatePaymentCategoriesCommand(id, staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new PaymentCategoriesCqrs.DeletePaymentCategoriesCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}