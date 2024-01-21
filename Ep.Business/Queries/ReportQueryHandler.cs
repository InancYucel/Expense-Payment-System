using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class ReportQueryHandler : 
    IRequestHandler<ReportCqrs.GetStaffExpenseReportWithStaffId, ApiResponse<List<ExpensesResponse>>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public ReportQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
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
}