using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class PaymentCategoriesQueryHandler : 
    IRequestHandler<PaymentCategoriesCqrs.GetAllPaymentCategoriesQuery, ApiResponse<List<PaymentCategoriesResponse>>>,
    IRequestHandler<PaymentCategoriesCqrs.GetPaymentCategoriesByIdQuery, ApiResponse<PaymentCategoriesResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public PaymentCategoriesQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<PaymentCategoriesResponse>>> Handle(PaymentCategoriesCqrs.GetAllPaymentCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<PaymentCategories>().Where(x => x.IsActive == true).ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<PaymentCategories>, List<PaymentCategoriesResponse>>(list);
        return new ApiResponse<List<PaymentCategoriesResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<PaymentCategoriesResponse>> Handle(PaymentCategoriesCqrs.GetPaymentCategoriesByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<PaymentCategories>().FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive == true, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<PaymentCategoriesResponse>("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        var mapped = _mapper.Map<PaymentCategories, PaymentCategoriesResponse>(entity);
        return new ApiResponse<PaymentCategoriesResponse>(mapped);
    }
}