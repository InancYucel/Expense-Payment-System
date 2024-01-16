using Base.Response;
using Business.Cqrs;
using Data.Insert;
using MediatR;
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
    public async Task<ApiResponse<List<ExpensesResponse>>> Get()
    {
        var operation = new ExpensesCqrs.GetAllExpensesQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ApiResponse<ExpensesResponse>> Get(int id)
    {
        var operation = new ExpensesCqrs.GetExpensesByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    public async Task<ApiResponse<ExpensesResponse>> Post([FromBody] ExpensesRequest account)
    {
        var operation = new ExpensesCqrs.CreateExpensesCommand(account);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse> Put(int id, [FromBody] ExpensesRequest account)
    {
        var operation = new ExpensesCqrs.UpdateExpensesCommand(id, account);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new ExpensesCqrs.DeleteExpensesCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}