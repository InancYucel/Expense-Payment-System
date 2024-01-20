using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Business.DbExistControls;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class StaffCommandHandler : //Mediator Interfaces
    IRequestHandler<StaffCqrs.CreateStaffCommand, ApiResponse<StaffResponse>>,
    IRequestHandler<StaffCqrs.UpdateStaffCommand,ApiResponse>,
    IRequestHandler<StaffCqrs.DeleteStaffCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly StaffExist _staffExist;

    public StaffCommandHandler(EpDbContext dbContext, IMapper mapper) //DI for dbContext and mapper
    {
        _dbContext = dbContext; //DI
        _mapper = mapper; //DI
        _staffExist = new StaffExist(_dbContext); // Create it once throughout the class
    }

    public async Task<ApiResponse<StaffResponse>> Handle(StaffCqrs.CreateStaffCommand request, CancellationToken cancellationToken)
    {
        if (_staffExist.IsStaffExist(request.Model.Id)) //Checking whether StaffId is already registered in the system.
        {
            return new ApiResponse<StaffResponse>("This Staff ID is already registered in the system");
        }
        var entity = _mapper.Map<StaffRequest, Staff>(request.Model);
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
            return new ApiResponse("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        if (_staffExist.IsStaffExist(request.Model.Id)) //Checking whether Staff ID is already registered in the system.
        {
            return new ApiResponse("This Staff ID is already registered in the system");
        }
        
        if (_staffExist.IsIdentityNumberExist(request.Model.IdentityNumber)) //Checking whether Identity Number is already registered in the system.
        {
            return new ApiResponse("This IdentityNumber is already registered in the system");
        }
        
        fromDb.FirstName = request.Model.FirstName;
        fromDb.LastName = request.Model.LastName;
        fromDb.IdentityNumber = request.Model.IdentityNumber;
        fromDb.LastActivityDate = request.Model.LastActivityDate;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(StaffCqrs.DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Staff>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        
        if (fromDb == null)
        {
            return new ApiResponse("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}