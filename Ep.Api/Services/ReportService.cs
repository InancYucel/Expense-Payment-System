using System.Globalization;
using AspNetCore.Reporting;
using System.Reflection;
using System.Text;
using Base.Response;
using Data.DbContext;
using Data.Entity;
using Schema;

namespace Expense_Payment_System.Services;

public interface IReportService
{
	byte[] GenerateReportForStaffExpenseReportAsync(string reportName, string reportType, ApiResponse<List<ExpensesResponse>> expensesResponse);

    List<ReportService.MonthCategory> SeparateByMonthCategory(List<ExpensePaymentOrderResponse> reponse);

    byte[] GenerateReportForPaymentIntensityAsync(string reportName, string reportType,List<ReportService.MonthCategory> response);
}

public class ReportService : IReportService
{
    private readonly EpDbContext _dbContext;

	public ReportService(EpDbContext dbContext)
	{
        _dbContext = dbContext;
	}

    public byte[] GenerateReportForStaffExpenseReportAsync(string reportName, string reportType, ApiResponse<List<ExpensesResponse>> expensesResponse)
    {
        var fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\net7.0\\Ep.Api.dll", string.Empty);
        var rdlcFilePath = String.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding.GetEncoding("utf-8");

        var report = new LocalReport(rdlcFilePath);

        // Prepare data for report
        report.AddDataSource("dsUsers", expensesResponse.Response);
        var parameters = new Dictionary<string, string>();
        var result = report.Execute(GetRenderType(reportType), 1, parameters);

        return result.MainStream;
    }
    
    private RenderType GetRenderType(string reportType)
    {
        var renderType = RenderType.Pdf;

        switch (reportType.ToUpper())
        {
            default:
            case "PDF":
                renderType = RenderType.Pdf;
                break;
            case "XLS":
                renderType = RenderType.Excel;
                break;
            case "WORD":
                renderType = RenderType.Word;
                break;
        }

        return renderType;
    }
    

    public List<MonthCategory> SeparateByMonthCategory(List<ExpensePaymentOrderResponse> response)
    {
        var result = new List<MonthCategory>();

        for (var i = 0; i < response.Count; i++)
        {
            var monthR = response[i].PaymentCompletedDate.ToString("MMMM", CultureInfo.InvariantCulture);
            var amountR = _dbContext.Set<Expenses>().FirstOrDefault(x => x.Id == response[i].ExpenseId).InvoiceAmount;
            var isExist = result.Find(x => x.month == monthR);
            if (isExist is null)
            {
                result.Add(new MonthCategory()
                {
                    month = monthR,
                    paymentTotal = amountR,
                    numberOfPayments = 1
                });
            }
            else
            {
                isExist.numberOfPayments = isExist.numberOfPayments + 1;
                isExist.paymentTotal = isExist.paymentTotal + amountR;
            }
        }
        return result;
    }
    
    public byte[] GenerateReportForPaymentIntensityAsync(string reportName, string reportType, List<MonthCategory> response)
    {
        var fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\net7.0\\Ep.Api.dll", string.Empty);
        var rdlcFilePath = String.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding.GetEncoding("utf-8");

        var report = new LocalReport(rdlcFilePath);

        // Prepare data for report
        report.AddDataSource("dsUsers", response);
        var parameters = new Dictionary<string, string>();
        var result = report.Execute(GetRenderType(reportType), 1, parameters);

        return result.MainStream;
    }
    
    public class MonthCategory
    {
        public string month;
        public int numberOfPayments;
        public double paymentTotal;
    }
}
