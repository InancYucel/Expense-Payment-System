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

    public ExpensesController(IMediator mediator) //Dependency injection for Mediator
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] //Specifies that only users with the admin role can enter
    public async Task<ApiResponse<List<ExpensesResponse>>> Get()
    {
        var operation = new ExpensesCqrs.GetAllExpensesQuery();
        var result = await _mediator.Send(operation); //Mediator keeps the Colleague references within which it will communicate and provides the necessary functionality.
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
    public async Task<ApiResponse> Put(int id, [FromBody] ExpensesRequestForUpdate expense)
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")] //Specifies that only users with the staff role can enter
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
    
    [HttpGet("FilterExpenseWithRequestStatus/{staffId:int}, {requestStatus}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse<List<ExpensesResponse>>> FilterExpenseWithRequestStatus(int staffId, string requestStatus)
    {
        if (requestStatus.ToLower() is not ("approved" or "waiting" or "denied"))
        {
            return new ApiResponse<List<ExpensesResponse>>(
                "request status can only be 'approved' - 'waiting' - 'denied'");
        }
        var operation = new ExpensesCqrs.FilterExpenseWithRequestStatus(staffId, requestStatus);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("FilterExpenseWithInvoiceAmount/{staffId:int}, {invoiceAmountBegin:double}, {invoiceAmountEnd:double}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse<List<ExpensesResponse>>> FilterExpenseWithInvoiceAmount(int staffId, double invoiceAmountBegin, double invoiceAmountEnd)
    {
        var operation = new ExpensesCqrs.FilterExpenseWithInvoiceAmount(staffId, invoiceAmountBegin, invoiceAmountEnd);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("GetRejectedRefundRequests/{staffId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse<List<ExpensesResponse>>> GetRejectedRefundRequests(int staffId)
    {
        var operation = new ExpensesCqrs.GetRejectedRefundRequests(staffId);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPut("ReplyToApplication/{expenseId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse> ReplyToApplication([FromBody] ReplyExpensesRequest expenseBody, int expenseId)
    {
        var operation = new ExpensesCqrs.ReplyToApplication(expenseId, expenseBody);
        var result = await _mediator.Send(operation);
        return result;
    }
}