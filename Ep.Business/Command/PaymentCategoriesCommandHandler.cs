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

public class PaymentCategoriesCommandHandler : //Mediator Interfaces
    IRequestHandler<PaymentCategoriesCqrs.CreatePaymentCategoriesCommand, ApiResponse<PaymentCategoriesResponse>>,
    IRequestHandler<PaymentCategoriesCqrs.UpdatePaymentCategoriesCommand,ApiResponse>,
    IRequestHandler<PaymentCategoriesCqrs.DeletePaymentCategoriesCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CategoryExist _categoryExist;

    public PaymentCategoriesCommandHandler(EpDbContext dbContext, IMapper mapper) //DI for dbContext and mapper
    {
        _dbContext = dbContext; //DI
        _mapper = mapper; //DI
        _categoryExist = new CategoryExist(_dbContext); // Create it once throughout the class
    }

    public async Task<ApiResponse<PaymentCategoriesResponse>> Handle(PaymentCategoriesCqrs.CreatePaymentCategoriesCommand request, CancellationToken cancellationToken)
    {
        if(_categoryExist.IsCategoryExist(request.Model.Category))
        {
            return new ApiResponse<PaymentCategoriesResponse>("This Category is already added");
        }
        var entity = _mapper.Map<PaymentCategoriesRequest, PaymentCategories>(request.Model);
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var mapped = _mapper.Map<PaymentCategories, PaymentCategoriesResponse>(entityResult.Entity);
        return new ApiResponse<PaymentCategoriesResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(PaymentCategoriesCqrs.UpdatePaymentCategoriesCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<PaymentCategories>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        if(_categoryExist.IsCategoryExist(request.Model.Category))
        {
            return new ApiResponse("This Category is already added");
        }
        
        fromDb.Category = request.Model.Category;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(PaymentCategoriesCqrs.DeletePaymentCategoriesCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<PaymentCategories>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}