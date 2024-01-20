using AutoMapper;
using Base.Response;
using Business.Cqrs;
using Business.DbExistControls;
using Data.DbContext;
using Data.Entity;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Command;

public class AccountCommandHandler : 
    IRequestHandler<AccountCqrs.CreateAccountCommand, ApiResponse<AccountResponse>>,
    IRequestHandler<AccountCqrs.UpdateAccountCommand,ApiResponse>,
    IRequestHandler<AccountCqrs.DeleteAccountCommand,ApiResponse>
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly AccountExist _accountExist;
    private readonly StaffExist _staffExist;

    public AccountCommandHandler(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext; //DI
        _mapper = mapper; //DI
        _accountExist = new AccountExist(_dbContext);
        _staffExist = new StaffExist(_dbContext);
    }

    public async Task<ApiResponse<AccountResponse>> Handle(AccountCqrs.CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (_accountExist.IsIbanExist(request.Model.IBAN)) //Checking whether Iban is already registered in the system.
        {
            return new ApiResponse<AccountResponse>("This IBAN is already registered in the system");
        }
        if (_staffExist.IsStaffExist(request.Model.StaffId)) //Checking whether StaffId is already registered in the system.
        {
            return new ApiResponse<AccountResponse>("This StaffId is already registered in the system");
        }
        var entity = _mapper.Map<AccountRequest, Account>(request.Model); //Mapping RequestAccount to Account
        var entityResult = await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var mapped = _mapper.Map<Account, AccountResponse>(entityResult.Entity);  //Mapping Account to ResponseAccount
        return new ApiResponse<AccountResponse>(mapped);
    }

    public async Task<ApiResponse> Handle(AccountCqrs.UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Account>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        if (_accountExist.IsIbanExist(request.Model.IBAN)) //Checking whether Iban is already registered in the system.
        {
            return new ApiResponse("This IBAN is already registered in the system");
        }
        // Don't let them change the StaffId because it breaks the process
        fromDb.IBAN = request.Model.IBAN;
        fromDb.Bank = request.Model.Bank;
        fromDb.CurrencyType = request.Model.CurrencyType;
        fromDb.Name = request.Model.Name;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(AccountCqrs.DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await _dbContext.Set<Account>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromDb.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse("Soft Delete");
    }
}