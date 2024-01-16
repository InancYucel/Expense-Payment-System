using Base.Response;
using Business.Cqrs;
using Data.Insert;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace Expense_Payment_System.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<StaffResponse>>> Get()
    {
        var operation = new StaffCqrs.GetAllStaffQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ApiResponse<StaffResponse>> Get(int id)
    {
        var operation = new StaffCqrs.GetStaffByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    public async Task<ApiResponse<StaffResponse>> Post([FromBody] StaffRequest account)
    {
        var operation = new StaffCqrs.CreateStaffCommand(account);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse> Put(int id, [FromBody] StaffRequest account)
    {
        var operation = new StaffCqrs.UpdateStaffCommand(id, account);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new StaffCqrs.DeleteStaffCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}