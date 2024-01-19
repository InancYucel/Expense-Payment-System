using AutoMapper;
using Data.DbContext;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Functional;

public class MakePayment
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;

    public MakePayment(EpDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public bool CreateExpensePaymentOrder(int expenseId)
    {
        var expense = GetExpenseWithExpenseId(expenseId);
        var account = GetAccountInfos(expense);
        
        var newRow = new ExpensePaymentOrder();
        newRow.ExpenseId = expenseId;
        newRow.PaymentConfirmationDate = DateTime.Now;
        newRow.AccountConfirmingOrder = "admin";
        newRow.PaymentIban = account.IBAN; 
        _dbContext.Add(newRow); 
        _dbContext.SaveChanges();
        CreateTransaction(expense, account);
        return true;
    }

    public Account GetAccountInfos(Expenses expense)
    {
        var accounts = _dbContext.Set<Account>().Where(x => x.StaffId == expense.StaffId).ToList();
        
        for (var i = 0; i < accounts.Count; i++)
        {
            if (expense.InvoiceCurrencyType != accounts[i].CurrencyType)
            {
                accounts.RemoveAt(i);
            }
        }
        return accounts[0];
    }

    public Expenses GetExpenseWithExpenseId(int expenseId)
    {
        var entity =  _dbContext.Set<Expenses>().FirstOrDefault(x => x.Id == expenseId);
        return entity ?? null;
    }

    public void CreateTransaction(Expenses expense, Account account)
    {
        if (expense.InvoiceCurrencyType == "TRY")
        {
            CreateFastTransaction(expense, account);
        }
        else
        {
            CreateSwiftTransaction(expense, account);
        }
    }

    public void CreateFastTransaction(Expenses expense, Account account)
    {
        var companyAccount = _dbContext.Set<Account>().FirstOrDefault(x => x.StaffId == 4 && x.CurrencyType == expense.InvoiceCurrencyType);
        var row = new FastTransaction();
        row.AccountId = account.Id;
        row.ExpensePaymentOrderId = 
        row.ExpensePaymentOrderId = GetExpensePaymentOrderIdWithExpenseId(expense.Id);
        row.ReferenceNumber = new Random().Next(1000000, 9999999).ToString(); // TODO unique mi veritabanında daha önce oluşmuş mu diye kontrol edilmesi lazım
        row.TransactionDate = DateTime.Now;
        row.Amount = expense.InvoiceAmount;
        row.Description = expense.PaymentLocation;
        row.SenderBank = companyAccount.Bank;
        row.SenderIban = companyAccount.IBAN;
        row.SenderName = companyAccount.Name;
        row.ReceiverBank = account.Bank;
        row.ReceiverIban = account.IBAN;
        row.ReceiverName = account.Name;
        
        _dbContext.Add(row);
        var isSuccess = _dbContext.SaveChanges();
        if (isSuccess > 0)
        {
            UpdateExpensePaymentOrderRowsWithFast(row);
        }
    }
    
    public void CreateSwiftTransaction(Expenses expense, Account account)
    {
        var companyAccount = _dbContext.Set<Account>().FirstOrDefault(x => x.StaffId == 4 && x.CurrencyType == expense.InvoiceCurrencyType);
        var row = new SwiftTransaction();
        row.AccountId = account.Id;
        row.ExpensePaymentOrderId = GetExpensePaymentOrderIdWithExpenseId(expense.Id);
        row.ReferenceNumber = new Random().Next(1000000, 9999999).ToString(); // TODO unique mi veritabanında daha önce oluşmuş mu diye kontrol edilmesi lazım
        row.TransactionDate = DateTime.Now;
        row.Amount = expense.InvoiceAmount;
        row.CurrencyType = expense.InvoiceCurrencyType;
        row.Description = companyAccount.Name;
        row.SenderBank = companyAccount.Bank;
        row.SenderIban = companyAccount.IBAN;
        row.ReceiverBank = account.Bank;
        row.ReceiverIban = account.IBAN;
        row.ReceiverName = account.Name;
        
        _dbContext.Add(row);
        var isSuccess = _dbContext.SaveChanges();
        if (isSuccess > 0)
        {
            UpdateExpensePaymentOrderRowsWithSwift(row);
        }
    }

    public int GetExpensePaymentOrderIdWithExpenseId(int expenseId)
    {
        var row = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.ExpenseId == expenseId);
        
        if (row == null)
        {
            return 0;
        }

        return row.Id;
    }

    public void UpdateExpensePaymentOrderRowsWithFast(FastTransaction fastTransaction)
    {
        var entity = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.Id == fastTransaction.ExpensePaymentOrderId);
        entity.IsPaymentCompleted = true;
        entity.PaymentCompletedDate = DateTime.Now;
        _dbContext.SaveChanges();
    }
    
    public void UpdateExpensePaymentOrderRowsWithSwift(SwiftTransaction swiftTransaction)
    {
        var entity = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.Id == swiftTransaction.ExpensePaymentOrderId);
        entity.IsPaymentCompleted = true;
        entity.PaymentCompletedDate = DateTime.Now;
        _dbContext.SaveChanges();
    }
}