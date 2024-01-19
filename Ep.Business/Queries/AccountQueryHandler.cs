using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class AccountQueryHandler : 
    IRequestHandler<AccountCqrs.GetAllAccountQuery, ApiResponse<List<AccountResponse>>>,
    IRequestHandler<AccountCqrs.GetAccountByIdQuery, ApiResponse<AccountResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public AccountQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<AccountResponse>>> Handle(AccountCqrs.GetAllAccountQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Account>().ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<Account>, List<AccountResponse>>(list);
        return new ApiResponse<List<AccountResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<AccountResponse>> Handle(AccountCqrs.GetAccountByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<AccountResponse>("Record not found");
        }
        
        var mapped = _mapper.Map<Account, AccountResponse>(entity);
        return new ApiResponse<AccountResponse>(mapped);
    }
}