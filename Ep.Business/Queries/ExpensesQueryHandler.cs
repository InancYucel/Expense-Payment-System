using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class ExpensesQueryHandler : 
    IRequestHandler<ExpensesCqrs.GetAllExpensesQuery, ApiResponse<List<ExpensesResponse>>>,
    IRequestHandler<ExpensesCqrs.GetExpenseByIdQuery, ApiResponse<ExpensesResponse>>,
    IRequestHandler<ExpensesCqrs.GetExpenseByStaffIdQuery, ApiResponse<List<ExpensesResponse>>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpensesQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<List<ExpensesResponse>>> Handle(ExpensesCqrs.GetAllExpensesQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Expenses>().ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<Expenses>, List<ExpensesResponse>>(list);
        return new ApiResponse<List<ExpensesResponse>>(mappedList);
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<ApiResponse<ExpensesResponse>> Handle(ExpensesCqrs.GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<Expenses>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<ExpensesResponse>("Record not found");
        }
        
        var mapped = _mapper.Map<Expenses, ExpensesResponse>(entity);
        return new ApiResponse<ExpensesResponse>(mapped);
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")]
    public async Task<ApiResponse<List<ExpensesResponse>>> Handle(ExpensesCqrs.GetExpenseByStaffIdQuery request,CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Expenses>().Where(x => x.StaffId == request.StaffId).ToListAsync(cancellationToken);

        if (!list.Any())
        {
            return new ApiResponse<List<ExpensesResponse>>("Record not found");
        }
        var mappedList = _mapper.Map<List<Expenses>, List<ExpensesResponse>>(list);
        return new ApiResponse<List<ExpensesResponse>>(mappedList);
    }
}