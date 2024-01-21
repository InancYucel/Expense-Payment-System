using AutoMapper;
using Data.DbContext;

namespace Business.Functional;

public class ReportCalculate
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ReportCalculate(EpDbContext dbContext, IMapper mapper) //Dependency Injection for _dbContext, _mapper
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}