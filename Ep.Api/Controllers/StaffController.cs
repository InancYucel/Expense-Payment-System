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
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffController(IMediator mediator) //Dependency injection for Mediator
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] //Specifies that only users with the admin role can enter
    public async Task<ApiResponse<List<StaffResponse>>> Get()
    {
        var operation = new StaffCqrs.GetAllStaffQuery();
        var result = await _mediator.Send(operation); //Mediator keeps the Colleague references within which it will communicate and provides the necessary functionality.
        return result;
    }
    
    [HttpGet("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<StaffResponse>> Get(int id)
    {
        var operation = new StaffCqrs.GetStaffByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<StaffResponse>> Post([FromBody] StaffRequest staffRequest)
    {
        var operation = new StaffCqrs.CreateStaffCommand(staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] StaffRequest staffRequest)
    {
        var operation = new StaffCqrs.UpdateStaffCommand(id, staffRequest);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new StaffCqrs.DeleteStaffCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}