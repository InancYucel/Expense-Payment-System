using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class AccountCqrs
{
    public record GetAllAccountQuery() : IRequest<ApiResponse<List<AccountResponse>>>;
    public record GetAccountByIdQuery(int Id) : IRequest<ApiResponse<AccountResponse>>;
    public record DeleteAccountCommand(int Id) : IRequest<ApiResponse>;
    public record UpdateAccountCommand(int Id,AccountRequestForUpdate Model) : IRequest<ApiResponse>;
    public record CreateAccountCommand(AccountRequest Model) : IRequest<ApiResponse<AccountResponse>>;
}