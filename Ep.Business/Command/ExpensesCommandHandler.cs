using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class ExpensesCommandHandler : 
    IRequestHandler<ExpensesCqrs.CreateExpensesCommand, ApiResponse<ExpensesResponse>>,
    IRequestHandler<ExpensesCqrs.UpdateExpensesCommand,ApiResponse>,
    IRequestHandler<ExpensesCqrs.DeleteExpensesCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpensesCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ExpensesResponse>> Handle(ExpensesCqrs.CreateExpensesCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ExpensesRequest, Expenses>(request.Model);
        // TODO Staff tablosunda bulunmayan ID'li bir staffId verilince patlıyor kontrol edilmeli fluent validation ile
        
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var mapped = _mapper.Map<Expenses, ExpensesResponse>(entityResult.Entity);
        return new ApiResponse<ExpensesResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(ExpensesCqrs.UpdateExpensesCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Expenses>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        // TODO update ederken zaten halihazırda db de olan bir id ile update edince hata dönüyor bunu handle edip kontrol edebilirsin
        // TODO staff number key olduğu için değiştirilemiyor
        
        fromDb.InvoiceReferenceNumber = request.Model.InvoiceReferenceNumber;
        fromDb.InvoiceAmount = request.Model.InvoiceAmount;
        fromDb.InvoiceDescription = request.Model.InvoiceDescription;
        fromDb.ExpenseClaimDescription = request.Model.ExpenseClaimDescription;
        fromDb.ExpenseRequestStatus = request.Model.ExpenseRequestStatus;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(ExpensesCqrs.DeleteExpensesCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Expenses>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}