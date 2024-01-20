using AutoMapper;
using Business.DbExistControls;
using Data.DbContext;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Business.Functional;

public class MakePayment
{
    private readonly EpDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TransactionExist _transactionExist;

    public MakePayment(EpDbContext dbContext, IMapper mapper) //Dependency Injection for _dbContext, _mapper
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _transactionExist = new TransactionExist(_dbContext); //Created once
    }

    public void CreateExpensePaymentOrder(int expenseId, double modelAmount)
    {
        // First, an expense payment order is created.
        var expense = GetExpenseWithExpenseId(expenseId);
        var account = GetAccountInfos(expense);
        
        var newRow = new ExpensePaymentOrder
        {
            ExpenseId = expenseId,
            PaymentConfirmationDate = DateTime.Now,
            AccountConfirmingOrder = "admin",
            PaymentIban = account.IBAN,
            PaymentCategory = expense.InvoiceCategory
        };
        _dbContext.Add(newRow); 
        _dbContext.SaveChanges();
        CreateTransaction(expense, account, modelAmount);
    }

    private Account GetAccountInfos(Expenses expense)
    {
        // There is an Account row that is connected to the staffId in the Expenses table.
        
        var accounts = _dbContext.Set<Account>().Where(x => x.StaffId == expense.StaffId).ToList();
        
        //There are multiple accounts with the same Staff ID. Some of them are TRY, some are USD, some are EUR. We do this to find the account in the currency type we want.
        for (var i = 0; i < accounts.Count; i++)
        {
            if (expense.InvoiceCurrencyType != accounts[i].CurrencyType)
            {
                accounts.RemoveAt(i);
            }
        }
        return accounts[0];
    }

    private Expenses GetExpenseWithExpenseId(int expenseId)
    {
        // The row related to ExpenseId is fetched
        var entity =  _dbContext.Set<Expenses>().FirstOrDefault(x => x.Id == expenseId);
        return entity ?? null;
    }

    private void CreateTransaction(Expenses expense, Account account, double modelAmount)
    {
        // If the money transferred is TRY, Fast payment is made; If it is foreign currency, Swift payment is made.
        if (expense.InvoiceCurrencyType == "TRY")
        {
            CreateFastTransaction(expense, account, modelAmount);
        }
        else
        {
            CreateSwiftTransaction(expense, account, modelAmount);
        }
    }

    private void CreateFastTransaction(Expenses expense, Account receiverAccount, double modelAmount)
    {
        //The reference number we randomly created may already be in the table, we do this check to prevent this situation.
        var randomReferenceNumber = new Random().Next(100000, 999999).ToString();
        if (_transactionExist.IsReferenceExistInFastTransaction(randomReferenceNumber))
        {
            randomReferenceNumber = new Random().Next(100000, 999999).ToString();
        }
        
        // ID of sender accounts, i.e. company accounts, is 4
        // TODO eğer jsonsuz ayağa kaldırılır ise ödemenin çıkacağı hesap bulunamayacak, bir çözüm ?
        var senderAccount = _dbContext.Set<Account>().FirstOrDefault(x => x.StaffId == 4 && x.CurrencyType == expense.InvoiceCurrencyType);
        var row = new FastTransaction
        {
            AccountId = receiverAccount.Id,
            ExpensePaymentOrderId = GetExpensePaymentOrderIdWithExpenseId(expense.Id),
            ReferenceNumber = randomReferenceNumber,
            TransactionDate = DateTime.Now,
            Amount = modelAmount,
            Description = expense.PaymentLocation,
            SenderBank = senderAccount.Bank,
            SenderIban = senderAccount.IBAN,
            SenderName = senderAccount.Name,
            ReceiverBank = receiverAccount.Bank,
            ReceiverIban = receiverAccount.IBAN,
            ReceiverName = receiverAccount.Name
        };

        var isSuccess = MoneyOutAndIn(receiverAccount, senderAccount, row.Amount);
        if (!isSuccess) // If the money transfer is successful, continue
        {
            return;
        }
        
        _dbContext.Add(row);
        var isSuccessSave = _dbContext.SaveChanges();
        if (isSuccessSave > 0)
        {
            UpdateExpensePaymentOrderRowsWithFast(row);
        }
    }
    
    private void CreateSwiftTransaction(Expenses expense, Account receiverAccount, double modelAmount)
    {
        // //The reference number we randomly created may already be in the table, we do this check to prevent this situation.
        var randomReferenceNumber = new Random().Next(1000000, 9999999).ToString();
        if (_transactionExist.IsReferenceExistInSwiftTransaction(randomReferenceNumber))
        {
            randomReferenceNumber = new Random().Next(1000000, 9999999).ToString();
        }
        // TODO eğer jsonsuz ayağa kaldırılır ise ödemenin çıkacağı hesap bulunamayacak, bir çözüm ?
        var senderAccount = _dbContext.Set<Account>().FirstOrDefault(x => x.StaffId == 4 && x.CurrencyType == expense.InvoiceCurrencyType);
        var row = new SwiftTransaction
        {
            AccountId = receiverAccount.Id,
            ExpensePaymentOrderId = GetExpensePaymentOrderIdWithExpenseId(expense.Id),
            ReferenceNumber = randomReferenceNumber,
            TransactionDate = DateTime.Now,
            Amount = modelAmount,
            CurrencyType = expense.InvoiceCurrencyType,
            Description = senderAccount.Name,
            SenderBank = senderAccount.Bank,
            SenderIban = senderAccount.IBAN,
            ReceiverBank = receiverAccount.Bank,
            ReceiverIban = receiverAccount.IBAN,
            ReceiverName = receiverAccount.Name
        };

        var isSuccess = MoneyOutAndIn(receiverAccount, senderAccount, row.Amount);
        if (!isSuccess)  // If the money transfer is successful, continue
        {
            return;
        }
        
        _dbContext.Add(row);
        
        var isSuccessSave = _dbContext.SaveChanges();
        if (isSuccessSave > 0)
        {
            UpdateExpensePaymentOrderRowsWithSwift(row);
        }
    }

    private int GetExpensePaymentOrderIdWithExpenseId(int expenseId)
    {
        // Expense ID is used to find the row in the ExpensePaymentOrder table
        var row = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.ExpenseId == expenseId);
        return row?.Id ?? 0;
    }

    private void UpdateExpensePaymentOrderRowsWithFast(FastTransaction fastTransaction)
    {
        // If the payment has been made, payment completed is printed on the payment order.
        var entity = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.Id == fastTransaction.ExpensePaymentOrderId);
        entity.IsPaymentCompleted = true;
        entity.PaymentCompletedDate = DateTime.Now;
        _dbContext.SaveChanges();
    }
    
    private void UpdateExpensePaymentOrderRowsWithSwift(SwiftTransaction swiftTransaction)
    {
        // If the payment has been made, payment completed is printed on the payment order.
        var entity = _dbContext.Set<ExpensePaymentOrder>().FirstOrDefault(x => x.Id == swiftTransaction.ExpensePaymentOrderId);
        entity.IsPaymentCompleted = true;
        entity.PaymentCompletedDate = DateTime.Now;
        _dbContext.SaveChanges();
    }

    private bool MoneyOutAndIn(Account receiverAccount, Account senderAccount, double amount)
    {
        var receiverAccountEntity = _dbContext.Set<Account>().FirstOrDefault(x => x.Id == receiverAccount.Id);
        var senderAccountEntity = _dbContext.Set<Account>().FirstOrDefault(x => x.Id == senderAccount.Id);
        // If one of the accounts is not found, the function ends.
        if (receiverAccountEntity == null || senderAccountEntity == null)
        {
            return false;
        }
        receiverAccountEntity.Balance = receiverAccountEntity.Balance + amount;
        _dbContext.SaveChanges();

        senderAccountEntity.Balance = senderAccountEntity.Balance - amount;
        _dbContext.SaveChanges();
        return true;
    }
}