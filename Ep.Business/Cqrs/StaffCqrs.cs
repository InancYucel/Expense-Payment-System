using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class StaffCqrs
{
    public record GetAllStaffQuery() : IRequest<ApiResponse<List<StaffResponse>>>;
    public record GetStaffByIdQuery(int Id) : IRequest<ApiResponse<StaffResponse>>;
    public record DeleteStaffCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateStaffCommand(int Id,StaffRequest Model) : IRequest<ApiResponse>;
    public record CreateStaffCommand(StaffRequest Model) : IRequest<ApiResponse<StaffResponse>>;
}