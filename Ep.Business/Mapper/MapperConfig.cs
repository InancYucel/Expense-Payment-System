using AutoMapper;
using Data.Entity;
using Schema;

namespace Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<StaffRequest, Staff>();
        CreateMap<Staff, StaffResponse>();
        
        CreateMap<ExpensesRequest, Expenses>();
        CreateMap<Expenses, ExpensesResponse>();
    }
}