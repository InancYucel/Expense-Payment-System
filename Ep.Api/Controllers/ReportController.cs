using Expense_Payment_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Business.Cqrs;
using Business.Functional;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Expense_Payment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMediator _mediator;
        public ReportController(IReportService reportService, IMediator mediator)
        {
            _reportService = reportService;
            _mediator = mediator;
        }

        [HttpGet("GetStaffExpenseReportWithStaffId{staffId:int}/{reportType}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "staff")] //Specifies that only users with the admin role can enter
        public async Task<ActionResult> GetStaffExpenseReportWithStaffId(int staffId, string reportType)
        {
            const string reportName = "StaffExpenseReport";
            var operation = new ReportCqrs.GetStaffExpenseReportWithStaffId(staffId);
            var result = await _mediator.Send(operation);
            if (result.Message == "Record not found")
            {
                return NotFound("Staff ID not found!");
            }
            var reportFileByteString = _reportService.GenerateReportForStaffExpenseReportAsync(reportName, reportType, result);
            return File(reportFileByteString, MediaTypeNames.Application.Octet, GetReportName(reportName, reportType));
        }
        
        [HttpGet("ReportPaymentIntensity{reportRangeType}/{reportYear:int}/{reportType}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] //Specifies that only users with the admin role can enter
        public async Task<ActionResult> ReportPaymentIntensity(string reportRangeType, int reportYear, string reportType)
        {
            if (reportRangeType.ToLower() is not ("daily" or "weekly" or "monthly"))
            {
                return NotFound("Report Range Type Must be 'Daily' or 'Weekly' or 'Monthly'");
            }
            var operation = new ReportCqrs.ReportPaymentIntensity(reportRangeType, reportYear);
            var result = await _mediator.Send(operation);

            if (reportRangeType is "monthly")
            {
                var reportResult = _reportService.SeparateByMonthCategory(result.Response);
                const string reportName = "PaymentIntensity";
                var reportFileByteString = _reportService.GenerateReportForPaymentIntensityAsync(reportName, reportType, reportResult);
                return File(reportFileByteString, MediaTypeNames.Application.Octet, GetReportName(reportName, reportType));
            }
            else if (reportRangeType is "weekly")
            {
                return NotFound("not completed");
            }
            else if (reportRangeType is "not completed")
            {
                return NotFound("Report Range Type Must be 'Daily' or 'Weekly' or 'Monthly'");
            }
            return NotFound("not completed");
        }

        private string GetReportName(string reportName, string reportType)
        {
            var outputFileName = reportName + ".pdf";

            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    outputFileName = reportName + ".pdf";
                    break;
                case "XLS":
                    outputFileName = reportName + ".xls";
                    break;
                case "DOC":
                    outputFileName = reportName + ".doc";
                    break;
            }

            return outputFileName;
        }

    }
}