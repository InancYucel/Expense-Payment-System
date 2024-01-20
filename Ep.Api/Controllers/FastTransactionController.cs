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
public class FastTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public FastTransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<List<FastTransactionResponse>>> Get()
    {
        var operation = new FastTransactionCqrs.GetAllFastTransactionQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<FastTransactionResponse>> Get(int id)
    {
        var operation = new FastTransactionCqrs.GetFastTransactionByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<FastTransactionResponse>> Post([FromBody] FastTransactionRequest staffRequest)
    {
        var operation = new FastTransactionCqrs.CreateFastTransactionCommand(staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] FastTransactionRequestForUpdate staffRequest)
    {
        var operation = new FastTransactionCqrs.UpdateFastTransactionCommand(id, staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new FastTransactionCqrs.DeleteFastTransactionCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}