using Expense_Payment_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Business.Cqrs;
using MediatR;

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

        [HttpGet("{staffId:int}/{reportType}")]
        public async Task<ActionResult> GetStaffExpenseReportWithStaffId(int staffId, string reportType)
        {
            var reportName = "StaffExpenseReport";
            var operation = new ReportCqrs.GetStaffExpenseReportWithStaffId(staffId);
            var result = await _mediator.Send(operation);
            var reportFileByteString = _reportService.GenerateReportAsync(reportName, reportType, result);
            return File(reportFileByteString, MediaTypeNames.Application.Octet, GetReportName(reportName, reportType));
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
