using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class FastTransactionQueryHandler : 
    IRequestHandler<FastTransactionCqrs.GetAllFastTransactionQuery, ApiResponse<List<FastTransactionResponse>>>,
    IRequestHandler<FastTransactionCqrs.GetFastTransactionByIdQuery, ApiResponse<FastTransactionResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public FastTransactionQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<FastTransactionResponse>>> Handle(FastTransactionCqrs.GetAllFastTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<FastTransaction>().ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<FastTransaction>, List<FastTransactionResponse>>(list);
        return new ApiResponse<List<FastTransactionResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<FastTransactionResponse>> Handle(FastTransactionCqrs.GetFastTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<FastTransaction>() .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<FastTransactionResponse>("Record not found");
        }
        
        var mapped = _mapper.Map<FastTransaction, FastTransactionResponse>(entity);
        return new ApiResponse<FastTransactionResponse>(mapped);
    }
}