using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class FastTransactionCqrs //Mediator Records
{
    public record GetAllFastTransactionQuery() : IRequest<ApiResponse<List<FastTransactionResponse>>>;
    public record GetFastTransactionByIdQuery(int Id) : IRequest<ApiResponse<FastTransactionResponse>>;
    public record DeleteFastTransactionCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateFastTransactionCommand(int Id,FastTransactionRequestForUpdate Model) : IRequest<ApiResponse>;
    public record CreateFastTransactionCommand(FastTransactionRequest Model) : IRequest<ApiResponse<FastTransactionResponse>>;
}