using FileExport.Api.interfaces;
using FileExport.Api.Model;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileExport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsReportDemoController : ControllerBase
    {
        private readonly IExportFile _exportFile;

        public JsReportDemoController(IExportFile exportFile)
        {
            _exportFile = exportFile;
        }

        [HttpGet("list-to-excel")]
        public FileResult Export()
        {
            GenerateData generator = new GenerateData();
            var data = generator.GenerateRandomData();

            var bytes = _exportFile.ExportExcel(data);
            return File(bytes, "application/vnd.ms-excel", $"File_{DateTime.Now:yyyy-MM-dd_HH-mm}.xlsx");
        }

        [HttpGet("html-to-excel")]
        public FileResult ExportExcel(string html)
        {
            var bytes =  _exportFile.ExportExcel(html);
            return File(bytes, "application/vnd.ms-excel", $"File_{DateTime.Now:yyyy-MM-dd_HH-mm}.xlsx");
        }

        [HttpGet("html-to-pdf")]
        public FileResult ExportPdf(string html)
        {
            var bytes = _exportFile.ExportPdf(html);
            return File(bytes, "application/vnd.ms-pdf", $"File_{DateTime.Now:yyyy-MM-dd_HH-mm}.pdf");
        }
    }
}
