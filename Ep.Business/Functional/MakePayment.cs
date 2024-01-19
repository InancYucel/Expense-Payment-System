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

    public bool CreateExpensePaymentOrder(int expenseId, double modelAmount)
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
        CreateTransaction(expense, account, modelAmount);
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

    public void CreateTransaction(Expenses expense, Account account, double modelAmount)
    {
        if (expense.InvoiceCurrencyType == "TRY")
        {
            CreateFastTransaction(expense, account, modelAmount);
        }
        else
        {
            CreateSwiftTransaction(expense, account, modelAmount);
        }
    }

    public void CreateFastTransaction(Expenses expense, Account receiverAccount, double modelAmount)
    {
        var senderAccount = _dbContext.Set<Account>().FirstOrDefault(x => x.StaffId == 4 && x.CurrencyType == expense.InvoiceCurrencyType);
        var row = new FastTransaction();
        row.AccountId = receiverAccount.Id;
        row.ExpensePaymentOrderId = 
        row.ExpensePaymentOrderId = GetExpensePaymentOrderIdWithExpenseId(expense.Id);
        row.ReferenceNumber = new Random().Next(1000000, 9999999).ToString(); // TODO unique mi veritabanında daha önce oluşmuş mu diye kontrol edilmesi lazım
        row.TransactionDate = DateTime.Now;
        row.Amount = modelAmount;
        row.Description = expense.PaymentLocation;
        row.SenderBank = senderAccount.Bank;
        row.SenderIban = senderAccount.IBAN;
        row.SenderName = senderAccount.Name;
        row.ReceiverBank = receiverAccount.Bank;
        row.ReceiverIban = receiverAccount.IBAN;
        row.ReceiverName = receiverAccount.Name;
        
        _dbContext.Add(row);
        var isSuccess = _dbContext.SaveChanges();
        if (isSuccess > 0)
        {
            MoneyOutAndIn(receiverAccount, senderAccount, row.Amount);
            UpdateExpensePaymentOrderRowsWithFast(row);
        }
    }
    
    public void CreateSwiftTransaction(Expenses expense, Account receiverAccount, double modelAmount)
    {
        var senderAccount = _dbContext.Set<Account>().FirstOrDefault(x => x.StaffId == 4 && x.CurrencyType == expense.InvoiceCurrencyType);
        var row = new SwiftTransaction();
        row.AccountId = receiverAccount.Id;
        row.ExpensePaymentOrderId = GetExpensePaymentOrderIdWithExpenseId(expense.Id);
        row.ReferenceNumber = new Random().Next(1000000, 9999999).ToString(); // TODO unique mi veritabanında daha önce oluşmuş mu diye kontrol edilmesi lazım
        row.TransactionDate = DateTime.Now;
        row.Amount = modelAmount;
        row.CurrencyType = expense.InvoiceCurrencyType;
        row.Description = senderAccount.Name;
        row.SenderBank = senderAccount.Bank;
        row.SenderIban = senderAccount.IBAN;
        row.ReceiverBank = receiverAccount.Bank;
        row.ReceiverIban = receiverAccount.IBAN;
        row.ReceiverName = receiverAccount.Name;
        
        _dbContext.Add(row);
        var isSuccess = _dbContext.SaveChanges();
        if (isSuccess > 0)
        {
            MoneyOutAndIn(receiverAccount, senderAccount, row.Amount);
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

    public void MoneyOutAndIn(Account receiverAccount, Account senderAccount, double amount)
    {
        var recieverAccountEntity = _dbContext.Set<Account>().FirstOrDefault(x => x.Id == receiverAccount.Id);
        recieverAccountEntity.Balance = recieverAccountEntity.Balance + amount;
        _dbContext.SaveChanges();

        var senderAccountEntity = _dbContext.Set<Account>().FirstOrDefault(x => x.Id == senderAccount.Id);
        senderAccountEntity.Balance = senderAccountEntity.Balance - amount;
        _dbContext.SaveChanges();
    }
}