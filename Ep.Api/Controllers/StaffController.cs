using Base.Response;
using Bussiness.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace Expense_Payment_System.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<StaffResponse>>> Get()
    {
        var operation = new StaffCqrs.GetAllStaffQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
}