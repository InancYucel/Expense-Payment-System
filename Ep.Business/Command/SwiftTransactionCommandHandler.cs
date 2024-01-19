using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class SwiftTransactionCommandHandler : 
    IRequestHandler<SwiftTransactionCqrs.CreateSwiftTransactionCommand, ApiResponse<SwiftTransactionResponse>>,
    IRequestHandler<SwiftTransactionCqrs.UpdateSwiftTransactionCommand,ApiResponse>,
    IRequestHandler<SwiftTransactionCqrs.DeleteSwiftTransactionCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public SwiftTransactionCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<SwiftTransactionResponse>> Handle(SwiftTransactionCqrs.CreateSwiftTransactionCommand request, CancellationToken cancellationToken)
    {
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
            return new ApiResponse("Record not found");
        }
        // TODO update ederken zaten halihazırda db de olan bir id ile update edince hata dönüyor bunu handle edip kontrol edebilirsin
        // TODO staff number key olduğu için değiştirilemiyor
        
        /*fromDb.FirstName = request.Model.FirstName;
        fromDb.LastName = request.Model.LastName;
        fromDb.Id = request.Model.Id;
        fromDb.IdentityNumber = request.Model.IdentityNumber;*/
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(SwiftTransactionCqrs.DeleteSwiftTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<SwiftTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}