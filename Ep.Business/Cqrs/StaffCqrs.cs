using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class StaffCqrs
{
    public record GetAllStaffQuery() : IRequest<ApiResponse<List<StaffResponse>>>;
}