using Base.Response;
using MediatR;
using Schema;

namespace Business.Cqrs;

public class PaymentCategoriesCqrs
{
    public record GetAllPaymentCategoriesQuery() : IRequest<ApiResponse<List<PaymentCategoriesResponse>>>;
    public record GetPaymentCategoriesByIdQuery(int Id) : IRequest<ApiResponse<PaymentCategoriesResponse>>;
    public record DeletePaymentCategoriesCommand(int Id) : IRequest<ApiResponse>;
    public record UpdatePaymentCategoriesCommand(int Id,PaymentCategoriesRequest Model) : IRequest<ApiResponse>;
    public record CreatePaymentCategoriesCommand(PaymentCategoriesRequest Model) : IRequest<ApiResponse<PaymentCategoriesResponse>>;
}