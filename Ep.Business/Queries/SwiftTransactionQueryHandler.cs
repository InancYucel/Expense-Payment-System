using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class SwiftTransactionQueryHandler : 
    IRequestHandler<SwiftTransactionCqrs.GetAllSwiftTransactionQuery, ApiResponse<List<SwiftTransactionResponse>>>,
    IRequestHandler<SwiftTransactionCqrs.GetSwiftTransactionByIdQuery, ApiResponse<SwiftTransactionResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public SwiftTransactionQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<SwiftTransactionResponse>>> Handle(SwiftTransactionCqrs.GetAllSwiftTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<SwiftTransaction>().Where(x => x.IsActive == true).ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<SwiftTransaction>, List<SwiftTransactionResponse>>(list);
        return new ApiResponse<List<SwiftTransactionResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<SwiftTransactionResponse>> Handle(SwiftTransactionCqrs.GetSwiftTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<SwiftTransaction>() .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive == true, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<SwiftTransactionResponse>("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        var mapped = _mapper.Map<SwiftTransaction, SwiftTransactionResponse>(entity);
        return new ApiResponse<SwiftTransactionResponse>(mapped);
    }
}