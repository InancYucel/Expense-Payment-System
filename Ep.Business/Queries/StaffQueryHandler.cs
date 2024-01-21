using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Queries;

public class StaffQueryHandler : 
    IRequestHandler<StaffCqrs.GetAllStaffQuery, ApiResponse<List<StaffResponse>>>,
    IRequestHandler<StaffCqrs.GetStaffByIdQuery, ApiResponse<StaffResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public StaffQueryHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<StaffResponse>>> Handle(StaffCqrs.GetAllStaffQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Staff>().Where(x => x.IsActive == true).ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<Staff>, List<StaffResponse>>(list);
        return new ApiResponse<List<StaffResponse>>(mappedList);
    }
    
    public async Task<ApiResponse<StaffResponse>> Handle(StaffCqrs.GetStaffByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<Staff>() .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive == true, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<StaffResponse>("Record not found"); // If there is no record to update, the function is canceled.
        }
        
        var mapped = _mapper.Map<Staff, StaffResponse>(entity);
        return new ApiResponse<StaffResponse>(mapped);
    }
}