using AutoMapper;
using Data.Entity;
using Schema;

namespace Business.Mapper;

public class MapperConfig : Profile
{
    // We record which models can be converted to which models in Mapper settings.
    public MapperConfig()
    {
        CreateMap<StaffRequest, Staff>();
        CreateMap<Staff, StaffResponse>();
        
        CreateMap<ExpensesRequest, Expenses>();
        CreateMap<StaffExpensesRequest, Expenses>();
        CreateMap<Expenses, ExpensesResponse>();
        
        CreateMap<AccountRequest, Account>();
        CreateMap<Account, AccountResponse>();
        
        CreateMap<ExpensePaymentOrderRequest, ExpensePaymentOrder>();
        CreateMap<ExpensePaymentOrder, ExpensePaymentOrderResponse>();
        
        CreateMap<FastTransactionRequest, FastTransaction>();
        CreateMap<FastTransaction, FastTransactionResponse>();
        
        CreateMap<SwiftTransactionRequest, SwiftTransaction>();
        CreateMap<SwiftTransaction, SwiftTransactionResponse>();
        
        CreateMap<PaymentCategoriesRequest, PaymentCategories>();
        CreateMap<PaymentCategories, PaymentCategoriesResponse>();
    }
}