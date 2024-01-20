using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class ExpensePaymentOrderQueryHandler : 
    IRequestHandler<ExpensePaymentOrderCqrs.GetAllExpensePaymentOrderQuery, ApiResponse<List<ExpensePaymentOrderResponse>>>,
    IRequestHandler<ExpensePaymentOrderCqrs.GetExpensePaymentOrderByIdQuery, ApiResponse<ExpensePaymentOrderResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpensePaymentOrderQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<ExpensePaymentOrderResponse>>> Handle(ExpensePaymentOrderCqrs.GetAllExpensePaymentOrderQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<ExpensePaymentOrder>().ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<ExpensePaymentOrder>, List<ExpensePaymentOrderResponse>>(list);
        return new ApiResponse<List<ExpensePaymentOrderResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<ExpensePaymentOrderResponse>> Handle(ExpensePaymentOrderCqrs.GetExpensePaymentOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<ExpensePaymentOrder>() .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<ExpensePaymentOrderResponse>("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        var mapped = _mapper.Map<ExpensePaymentOrder, ExpensePaymentOrderResponse>(entity);
        return new ApiResponse<ExpensePaymentOrderResponse>(mapped);
    }
}