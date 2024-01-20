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
public class SwiftTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SwiftTransactionController(IMediator mediator) //Dependency injection for Mediator
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] //Specifies that only users with the admin role can enter
    public async Task<ApiResponse<List<SwiftTransactionResponse>>> Get()
    {
        var operation = new SwiftTransactionCqrs.GetAllSwiftTransactionQuery();
        var result = await _mediator.Send(operation); //Mediator keeps the Colleague references within which it will communicate and provides the necessary functionality.
        return result;
    }
    
    [HttpGet("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<SwiftTransactionResponse>> Get(int id)
    {
        var operation = new SwiftTransactionCqrs.GetSwiftTransactionByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<SwiftTransactionResponse>> Post([FromBody] SwiftTransactionRequest staffRequest)
    {
        var operation = new SwiftTransactionCqrs.CreateSwiftTransactionCommand(staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] SwiftTransactionRequest staffRequest)
    {
        var operation = new SwiftTransactionCqrs.UpdateSwiftTransactionCommand(id, staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new SwiftTransactionCqrs.DeleteSwiftTransactionCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}