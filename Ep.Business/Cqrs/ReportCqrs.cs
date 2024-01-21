using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class ReportCqrs //Mediator Records
{
    public record GetStaffExpenseReportWithStaffId(int StaffId) : IRequest<ApiResponse<List<ExpensesResponse>>>;
    public record ReportPaymentIntensity(string ReportRangeType, int ReportYear) : IRequest<ApiResponse<List<ExpensePaymentOrderResponse>>>;

}