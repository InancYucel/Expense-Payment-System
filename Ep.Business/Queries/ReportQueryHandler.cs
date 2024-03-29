using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Business.Functional;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class ReportQueryHandler : 
    IRequestHandler<ReportCqrs.GetStaffExpenseReportWithStaffId, ApiResponse<List<ExpensesResponse>>>,
    IRequestHandler<ReportCqrs.ReportPaymentIntensity, ApiResponse<List<ExpensePaymentOrderResponse>>>
    
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ReportCalculate _reportCalculate;

    public ReportQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _reportCalculate = new ReportCalculate(_dbContext, _mapper);
    }
    
    public async Task<ApiResponse<List<ExpensesResponse>>> Handle(ReportCqrs.GetStaffExpenseReportWithStaffId request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Expenses>().Where(x => x.StaffId == request.StaffId).ToListAsync(cancellationToken);
        
        if (!list.Any())
        {
            return new ApiResponse<List<ExpensesResponse>>("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        var mappedList = _mapper.Map<List<Expenses>, List<ExpensesResponse>>(list);
        return new ApiResponse<List<ExpensesResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<List<ExpensePaymentOrderResponse>>> Handle(ReportCqrs.ReportPaymentIntensity request,
        CancellationToken cancellationToken)
    {
        var requestDateBegin = new DateTime(request.ReportYear, 1, 1);
        var requestDateEnd = new DateTime(request.ReportYear + 1, 1, 1);
        
        var list = await _dbContext.Set<ExpensePaymentOrder>().Where(x => x.PaymentCompletedDate >= requestDateBegin && x.PaymentCompletedDate < requestDateEnd)
            .OrderBy(o=>o.PaymentCompletedDate).ToListAsync(cancellationToken);
        
        if (!list.Any())
        {
            return new ApiResponse<List<ExpensePaymentOrderResponse>>("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        var mappedList = _mapper.Map<List<ExpensePaymentOrder>, List<ExpensePaymentOrderResponse>>(list);
        return new ApiResponse<List<ExpensePaymentOrderResponse>>(mappedList);
    }
}