using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class PaymentCategoriesCommandHandler : 
    IRequestHandler<PaymentCategoriesCqrs.CreatePaymentCategoriesCommand, ApiResponse<PaymentCategoriesResponse>>,
    IRequestHandler<PaymentCategoriesCqrs.UpdatePaymentCategoriesCommand,ApiResponse>,
    IRequestHandler<PaymentCategoriesCqrs.DeletePaymentCategoriesCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public PaymentCategoriesCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PaymentCategoriesResponse>> Handle(PaymentCategoriesCqrs.CreatePaymentCategoriesCommand request, CancellationToken cancellationToken)
    {
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
            return new ApiResponse("Record not found");
        }
        // TODO update ederken zaten halihazırda db de olan bir id ile update edince hata dönüyor bunu handle edip kontrol edebilirsin
        // TODO staff number key olduğu için değiştirilemiyor
        
        fromDb.Category = request.Model.Category;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(PaymentCategoriesCqrs.DeletePaymentCategoriesCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<PaymentCategories>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}