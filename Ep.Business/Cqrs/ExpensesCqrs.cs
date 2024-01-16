using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class ExpensesCqrs
{
    public record GetAllExpensesQuery() : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record GetExpensesByIdQuery(int Id) : IRequest<ApiResponse<ExpensesResponse>>;
    
    public record DeleteExpensesCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateExpensesCommand(int Id,ExpensesRequest Model) : IRequest<ApiResponse>;
    public record CreateExpensesCommand(ExpensesRequest Model) : IRequest<ApiResponse<ExpensesResponse>>;
}