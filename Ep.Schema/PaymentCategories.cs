using Base.Schema;

namespace Schema;

public class PaymentCategoriesRequest : BaseRequest
{
    public string Category { get; set; }
}

public class PaymentCategoriesResponse : BaseResponse
{
    public string Category { get; set; }
}