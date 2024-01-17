using Base.Response;
using Business.Cqrs;
using Data.Insert;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace Expense_Payment_System.Controllers;

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<List<ExpensesResponse>>> Get()
    {
        var operation = new ExpensesCqrs.GetAllExpensesQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<ExpensesResponse>> Get(int id)
    {
        var operation = new ExpensesCqrs.GetExpenseByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<ExpensesResponse>> Post([FromBody] ExpensesRequest expense)
    {
        var operation = new ExpensesCqrs.CreateExpenseCommand(expense);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] ExpensesRequest expense)
    {
        var operation = new ExpensesCqrs.UpdateExpenseCommand(id, expense);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new ExpensesCqrs.DeleteExpensesCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("GetExpenseWithStaffId/{staffId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse<List<ExpensesResponse>>> GetExpenseWithStaffId(int staffId)
    {
        var operation = new ExpensesCqrs.GetExpenseByStaffIdQuery(staffId);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost("CreateExpenseWithStaffId{staffId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse<ExpensesResponse>> CreateExpenseWithStaffId([FromBody] StaffExpensesRequest expenseBody, int staffId)
    {
        var operation = new ExpensesCqrs.CreateExpenseWithStaffIdCommand(expenseBody, staffId);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("UpdateOwnExpenseWithStaffId{staffId:int}, {expenseId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse> UpdateExpenseWithStaffId(int staffId, int expenseId, [FromBody] StaffExpensesRequest expenseBody)
    {
        var operation = new ExpensesCqrs.UpdateExpenseWithStaffIdCommand(staffId, expenseId,expenseBody);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpDelete("DeleteOwnExpenseWithStaffId{staffId:int}, {expenseId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse> DeleteExpenseWithStaffId(int staffId, int expenseId)
    {
        var operation = new ExpensesCqrs.DeleteExpenseWithStaffIdCommand(staffId, expenseId);
        var result = await _mediator.Send(operation);
        return result;
    }
}