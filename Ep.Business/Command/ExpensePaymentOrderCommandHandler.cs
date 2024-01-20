using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Business.DbExistControls;
using Data.DbContext;
using Data.Entity;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class ExpensePaymentOrderCommandHandler : 
    IRequestHandler<ExpensePaymentOrderCqrs.CreateExpensePaymentOrderCommand, ApiResponse<ExpensePaymentOrderResponse>>,
    IRequestHandler<ExpensePaymentOrderCqrs.UpdateExpensePaymentOrderCommand,ApiResponse>,
    IRequestHandler<ExpensePaymentOrderCqrs.DeleteExpensePaymentOrderCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CategoryExist _categoryExist;
    private readonly ExpensePaymentOrderExist _expensePaymentOrderExist;

    public ExpensePaymentOrderCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _expensePaymentOrderExist = new ExpensePaymentOrderExist(_dbContext);
    }

    public async Task<ApiResponse<ExpensePaymentOrderResponse>> Handle(ExpensePaymentOrderCqrs.CreateExpensePaymentOrderCommand request, CancellationToken cancellationToken)
    {
        if(!(_categoryExist.IsCategoryExist(request.Model.PaymentCategory)))
        {
            return new ApiResponse<ExpensePaymentOrderResponse>("This Category is not registered in the system");
        }
        var entity = _mapper.Map<ExpensePaymentOrderRequest, ExpensePaymentOrder>(request.Model);
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var mapped = _mapper.Map<ExpensePaymentOrder, ExpensePaymentOrderResponse>(entityResult.Entity);
        return new ApiResponse<ExpensePaymentOrderResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(ExpensePaymentOrderCqrs.UpdateExpensePaymentOrderCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<ExpensePaymentOrder>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        if(!(_categoryExist.IsCategoryExist(request.Model.PaymentCategory)))
        {
            return new ApiResponse("This Category is not registered in the system");
        }
        if(!(_expensePaymentOrderExist.IsExpenseIdIsExist(request.Model.ExpenseId)))
        {
            return new ApiResponse("This ExpenseId is not registered in the system");
        }
        fromDb.PaymentConfirmationDate = request.Model.PaymentConfirmationDate;
        fromDb.AccountConfirmingOrder = request.Model.AccountConfirmingOrder;
        fromDb.PaymentIban = request.Model.PaymentIban;
        fromDb.PaymentCategory = request.Model.PaymentCategory;
        fromDb.IsPaymentCompleted = request.Model.IsPaymentCompleted;
        fromDb.PaymentCompletedDate = request.Model.PaymentCompletedDate;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(ExpensePaymentOrderCqrs.DeleteExpensePaymentOrderCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<ExpensePaymentOrder>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}