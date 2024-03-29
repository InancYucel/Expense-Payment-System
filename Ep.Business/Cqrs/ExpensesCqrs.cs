using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class ExpensesCqrs //Mediator Records
{
    public record GetAllExpensesQuery() : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record GetExpenseByIdQuery(int Id) : IRequest<ApiResponse<ExpensesResponse>>;
    public record DeleteExpensesCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateExpenseCommand(int Id,ExpensesRequestForUpdate Model) : IRequest<ApiResponse>;
    public record CreateExpenseCommand(ExpensesRequest Model) : IRequest<ApiResponse<ExpensesResponse>>;
    
    public record GetExpenseByStaffIdQuery(int StaffId) : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record CreateExpenseWithStaffIdCommand(StaffExpensesRequest Model, int StaffId) : IRequest<ApiResponse<ExpensesResponse>>;
    public record UpdateExpenseWithStaffIdCommand(int StaffId, int ExpenseId, ExpensesRequestForUpdate Model) : IRequest<ApiResponse>;
    public record DeleteExpenseWithStaffIdCommand(int StaffId, int ExpenseId) : IRequest<ApiResponse>;
    public record FilterExpenseWithRequestStatus(int StaffId, string ExpenseRequestStatus) : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record FilterExpenseWithInvoiceAmount(int StaffId, double InvoiceAmountBegin, double InvoiceAmountEnd) : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record GetRejectedRefundRequests(int StaffId) : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record ReplyToApplication(int ExpenseId, ReplyExpensesRequest Model) : IRequest<ApiResponse>;
}