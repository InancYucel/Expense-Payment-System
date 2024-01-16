using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class ExpensesQueryHandler : 
    IRequestHandler<ExpensesCqrs.GetAllExpensesQuery, ApiResponse<List<ExpensesResponse>>>,
    IRequestHandler<ExpensesCqrs.GetExpensesByIdQuery, ApiResponse<ExpensesResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpensesQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<ExpensesResponse>>> Handle(ExpensesCqrs.GetAllExpensesQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Expenses>().ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<Expenses>, List<ExpensesResponse>>(list);
        return new ApiResponse<List<ExpensesResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<ExpensesResponse>> Handle(ExpensesCqrs.GetExpensesByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<Expenses>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<ExpensesResponse>("Record not found");
        }
        
        var mapped = _mapper.Map<Expenses, ExpensesResponse>(entity);
        return new ApiResponse<ExpensesResponse>(mapped);
    }
}