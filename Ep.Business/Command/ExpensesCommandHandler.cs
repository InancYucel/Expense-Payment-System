using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Business.Functional;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class ExpensesCommandHandler : 
    IRequestHandler<ExpensesCqrs.CreateExpenseCommand, ApiResponse<ExpensesResponse>>,
    IRequestHandler<ExpensesCqrs.UpdateExpenseCommand,ApiResponse>,
    IRequestHandler<ExpensesCqrs.DeleteExpensesCommand,ApiResponse>,
    IRequestHandler<ExpensesCqrs.CreateExpenseWithStaffIdCommand,ApiResponse<ExpensesResponse>>,
    IRequestHandler<ExpensesCqrs.UpdateExpenseWithStaffIdCommand,ApiResponse>,
    IRequestHandler<ExpensesCqrs.DeleteExpenseWithStaffIdCommand,ApiResponse>,
    IRequestHandler<ExpensesCqrs.ReplyToApplication,ApiResponse>
    
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpensesCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ExpensesResponse>> Handle(ExpensesCqrs.CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ExpensesRequest, Expenses>(request.Model);
        // TODO Staff tablosunda bulunmayan ID'li bir staffId verilince patlıyor kontrol edilmeli fluent validation ile
        
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var mapped = _mapper.Map<Expenses, ExpensesResponse>(entityResult.Entity);
        return new ApiResponse<ExpensesResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(ExpensesCqrs.UpdateExpenseCommand request, CancellationToken cancellationToken)
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
        fromDb.InvoiceCategory = request.Model.InvoiceCategory;
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
    
    public async Task<ApiResponse<ExpensesResponse>> Handle(ExpensesCqrs.CreateExpenseWithStaffIdCommand request, CancellationToken cancellationToken)
    {
        // TODO Staff tablosunda bulunmayan ID'li bir staffId verilince patlıyor kontrol edilmeli fluent validation ile
        var entity = _mapper.Map<StaffExpensesRequest, Expenses>(request.Model);
        entity.StaffId = request.StaffId;
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var mapped = _mapper.Map<Expenses, ExpensesResponse>(entityResult.Entity);
        return new ApiResponse<ExpensesResponse>(mapped);
    }
    
    public async Task<ApiResponse> Handle(ExpensesCqrs.UpdateExpenseWithStaffIdCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Expenses>().Where(x => x.StaffId == request.StaffId && x.Id == request.ExpenseId).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        // TODO update ederken zaten halihazırda db de olan bir id ile update edince hata dönüyor bunu handle edip kontrol edebilirsin
        // TODO staff number key olduğu için değiştirilemiyor
        
        fromDb.InvoiceReferenceNumber = request.Model.InvoiceReferenceNumber;
        fromDb.InvoiceAmount = request.Model.InvoiceAmount;
        fromDb.InvoiceCategory = request.Model.InvoiceCategory;
        fromDb.ExpenseClaimDescription = request.Model.ExpenseClaimDescription;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(ExpensesCqrs.DeleteExpenseWithStaffIdCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Expenses>().Where(x => x.StaffId == request.StaffId && x.Id == request.ExpenseId).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(ExpensesCqrs.ReplyToApplication request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Expenses>().Where(x => x.Id == request.ExpenseId).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }

        if (!(ExpenseStatusControl(fromDb.ExpenseRequestStatus.ToLower(), request.Model.ExpenseRequestStatus.ToLower())))
        {
            return new ApiResponse("Approved expense cannot be changed");
        }

        if (request.Model.ExpenseRequestStatus.ToLower() == "approved")
        {
            var expensePayment = new MakePayment(_dbContext, _mapper);
            expensePayment.CreateExpensePaymentOrder(request.ExpenseId, request.Model.InvoiceAmount);
        }
        fromDb.ExpenseRequestStatus = request.Model.ExpenseRequestStatus;
        fromDb.ExpensePaymentRefusal = request.Model.ExpensePaymentRefusal;
        
        var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

        if (saveResult > 0)
        {
            
        }
        return new ApiResponse();
    }

    public bool ExpenseStatusControl(string dbRequestStatus, string modelRequestStatus)
    {
        if (dbRequestStatus == "approved")
        {
            return false; // You cant change approved expense
        }
        else if (dbRequestStatus is "waiting" or "denied")
        {
            return true;
        }
        return true;
    }
}