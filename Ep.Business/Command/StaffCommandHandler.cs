using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class StaffCommandHandler : 
    IRequestHandler<StaffCqrs.CreateStaffCommand, ApiResponse<StaffResponse>>,
    IRequestHandler<StaffCqrs.UpdateStaffCommand,ApiResponse>,
    IRequestHandler<StaffCqrs.DeleteStaffCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public StaffCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<StaffResponse>> Handle(StaffCqrs.CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<StaffRequest, Staff>(request.Model);
        entity.Id = new Random().Next(10000000, 99999999); //TODO içerideki kayıtlarla çakışmasın kontrol lazım
        
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var mapped = _mapper.Map<Staff, StaffResponse>(entityResult.Entity);
        return new ApiResponse<StaffResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(StaffCqrs.UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Staff>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        // TODO update ederken zaten halihazırda db de olan bir id ile update edince hata dönüyor bunu handle edip kontrol edebilirsin
        // TODO staff number key olduğu için değiştirilemiyor
        
        fromDb.FirstName = request.Model.FirstName;
        fromDb.LastName = request.Model.LastName;
        fromDb.Id = request.Model.Id;
        fromDb.IdentityNumber = request.Model.IdentityNumber;
        fromDb.IBAN = request.Model.IBAN;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(StaffCqrs.DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Staff>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}