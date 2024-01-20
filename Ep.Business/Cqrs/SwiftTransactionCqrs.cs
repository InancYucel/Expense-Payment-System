using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class SwiftTransactionCqrs //Mediator Records
{
    public record GetAllSwiftTransactionQuery() : IRequest<ApiResponse<List<SwiftTransactionResponse>>>;
    public record GetSwiftTransactionByIdQuery(int Id) : IRequest<ApiResponse<SwiftTransactionResponse>>;
    public record DeleteSwiftTransactionCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateSwiftTransactionCommand(int Id,SwiftTransactionRequest Model) : IRequest<ApiResponse>;
    public record CreateSwiftTransactionCommand(SwiftTransactionRequest Model) : IRequest<ApiResponse<SwiftTransactionResponse>>;
}