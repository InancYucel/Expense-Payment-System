using AspNetCore.Reporting;
using System.Reflection;
using System.Text;
using Base.Response;
using Schema;

namespace Expense_Payment_System.Services;

public interface IReportService
{
	byte[] GenerateReportAsync(string reportName, string reportType, ApiResponse<List<ExpensesResponse>> expensesResponse);
}

public class ReportService : IReportService
{
	public ReportService()
	{
	}

    public byte[] GenerateReportAsync(string reportName, string reportType, ApiResponse<List<ExpensesResponse>> expensesResponse)
    {
        var fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("Ep.Api.dll", string.Empty);
        var rdlcFilePath = String.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding.GetEncoding("utf-8");

        var report = new LocalReport(rdlcFilePath);

        // Prepare data for report

        // expensesResponse.Response
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
}
