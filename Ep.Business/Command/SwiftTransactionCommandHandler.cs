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

public class SwiftTransactionCommandHandler : //Mediator Interfaces
    IRequestHandler<SwiftTransactionCqrs.CreateSwiftTransactionCommand, ApiResponse<SwiftTransactionResponse>>,
    IRequestHandler<SwiftTransactionCqrs.UpdateSwiftTransactionCommand,ApiResponse>,
    IRequestHandler<SwiftTransactionCqrs.DeleteSwiftTransactionCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ExpensePaymentOrderExist _expensePaymentOrderExist;
    private readonly TransactionExist _transactionExist;

    public SwiftTransactionCommandHandler(EpDbContext dbContext, IMapper mapper) //DI for dbContext and mapper
    {
        _dbContext = dbContext; //DI
        _mapper = mapper; //DI
        _expensePaymentOrderExist = new ExpensePaymentOrderExist(_dbContext); // Create it once throughout the class
        _transactionExist = new TransactionExist(_dbContext);
    }

    public async Task<ApiResponse<SwiftTransactionResponse>> Handle(SwiftTransactionCqrs.CreateSwiftTransactionCommand request, CancellationToken cancellationToken)
    {
        if(_expensePaymentOrderExist.IsExpensePaymentOrderIdIsExist(request.Model.ExpensePaymentOrderId))
        {
            return new ApiResponse<SwiftTransactionResponse>("This Expense Payment Order ID is registered in the system");
        }
        if(_transactionExist.IsReferenceNumberExistInSwiftTransaction(request.Model.ReferenceNumber))
        {
            return new ApiResponse<SwiftTransactionResponse>("This ReferenceNumber is registered in the system");
        }
        
        var entity = _mapper.Map<SwiftTransactionRequest, SwiftTransaction>(request.Model);
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var mapped = _mapper.Map<SwiftTransaction, SwiftTransactionResponse>(entityResult.Entity);
        return new ApiResponse<SwiftTransactionResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(SwiftTransactionCqrs.UpdateSwiftTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<SwiftTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found"); // If there is no record to update, the function is canceled.
        }
        if(_expensePaymentOrderExist.IsExpensePaymentOrderIdIsExist(request.Model.ExpensePaymentOrderId))
        {
            return new ApiResponse("This Expense Payment Order ID is registered in the system");
        }
        if(_transactionExist.IsReferenceNumberExistInSwiftTransaction(request.Model.ReferenceNumber))
        {
            return new ApiResponse("This ReferenceNumber is registered in the system");
        }
        
        fromDb.ReferenceNumber = request.Model.ReferenceNumber;
        fromDb.TransactionDate = request.Model.TransactionDate;
        fromDb.Amount = request.Model.Amount;
        fromDb.CurrencyType = request.Model.CurrencyType;
        fromDb.Description = request.Model.Description;
        fromDb.SenderBank = request.Model.SenderBank;
        fromDb.SenderIban = request.Model.SenderIban;
        fromDb.SenderName = request.Model.SenderName;
        fromDb.ReceiverBank = request.Model.ReceiverBank;
        fromDb.ReceiverIban = request.Model.ReceiverIban;
        fromDb.ReceiverName = request.Model.ReceiverName;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(SwiftTransactionCqrs.DeleteSwiftTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<SwiftTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}