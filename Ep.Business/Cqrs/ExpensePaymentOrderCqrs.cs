using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class ExpensePaymentOrderCqrs //Mediator Records
{
    public record GetAllExpensePaymentOrderQuery() : IRequest<ApiResponse<List<ExpensePaymentOrderResponse>>>;
    public record GetExpensePaymentOrderByIdQuery(int Id) : IRequest<ApiResponse<ExpensePaymentOrderResponse>>;
    public record DeleteExpensePaymentOrderCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateExpensePaymentOrderCommand(int Id,ExpensePaymentOrderRequest Model) : IRequest<ApiResponse>;
    public record CreateExpensePaymentOrderCommand(ExpensePaymentOrderRequest Model) : IRequest<ApiResponse<ExpensePaymentOrderResponse>>;
}