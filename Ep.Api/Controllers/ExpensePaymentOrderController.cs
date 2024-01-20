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
public class ExpensePaymentOrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensePaymentOrderController(IMediator mediator) //Dependency injection for Mediator
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] //Specifies that only users with the admin role can enter
    public async Task<ApiResponse<List<ExpensePaymentOrderResponse>>> Get()
    {
        var operation = new ExpensePaymentOrderCqrs.GetAllExpensePaymentOrderQuery();
        var result = await _mediator.Send(operation); //Mediator keeps the Colleague references within which it will communicate and provides the necessary functionality.
        return result;
    }
    
    [HttpGet("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<ExpensePaymentOrderResponse>> Get(int id)
    {
        var operation = new ExpensePaymentOrderCqrs.GetExpensePaymentOrderByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<ExpensePaymentOrderResponse>> Post([FromBody] ExpensePaymentOrderRequest staffRequest)
    {
        var operation = new ExpensePaymentOrderCqrs.CreateExpensePaymentOrderCommand(staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] ExpensePaymentOrderRequest staffRequest)
    {
        var operation = new ExpensePaymentOrderCqrs.UpdateExpensePaymentOrderCommand(id, staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new ExpensePaymentOrderCqrs.DeleteExpensePaymentOrderCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}