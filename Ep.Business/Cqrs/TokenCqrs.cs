using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public record CreateTokenCommand(TokenRequest Model) : IRequest<ApiResponse<TokenResponse>>;