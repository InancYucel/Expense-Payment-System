using Base.Response;
using Business.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace Expense_Payment_System.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator) //Dependency injection for Mediator
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    // Therefore, there is no authorization for user login.
    public async Task<ApiResponse<TokenResponse>> Post([FromBody] TokenRequest request)
    {
        var operation = new CreateTokenCommand(request);
        var result = await _mediator.Send(operation); //Mediator keeps the Colleague references within which it will communicate and provides the necessary functionality.
        return result;
    }
}