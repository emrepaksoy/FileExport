using ClosedXML.Excel;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;
using System.Reflection;



namespace FileExport.Api.interfaces

{
    public interface IExportFile
    {
        Byte[] ExportPdf(string html);
        Byte[] ExportExcel(string html);
        Byte[] ExportExcel<T>(List<T> data);

    }

    public class ExportFile : IExportFile
    {
        public Byte[] ExportExcel<T>(List<T> data)
        {
            System.Data.DataTable dt = new System.Data.DataTable(typeof(T).Name);

            PropertyInfo[] columns = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo pi in columns)
            {
                dt.Columns.Add(pi.Name);
            }

            foreach (var item in data)
            {
                var values = new object[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    values[i] = columns[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            byte[] bytes;
            using XLWorkbook xlFile = new XLWorkbook();

            xlFile.Worksheets.Add(dt, $"{typeof(T).Name}");
            
            using(MemoryStream ms = new MemoryStream())
            {
                xlFile.SaveAs(ms);
                bytes = ms.ToArray();
            }

            return bytes;
        }

        public Byte[] ExportExcel(string html)
        {
            var rs = new LocalReporting().UseBinary(JsReportBinary.GetBinary())
                 .KillRunningJsReportProcesses()
                 .AsUtility()
                 .Create();

            var report = rs.RenderAsync(new RenderRequest()
            {
                Template = new Template()
                {
                    Recipe = Recipe.HtmlToXlsx,
                    Engine = Engine.Handlebars,
                    Content = html
                }
            }).GetAwaiter().GetResult();

            var bytes = new byte[report.Content.Length];
            report.Content.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public Byte[] ExportPdf(string html)
        {
            var rs = new LocalReporting().UseBinary(JsReportBinary.GetBinary())
                .KillRunningJsReportProcesses()
                .AsUtility()
                .Create();

            var report = rs.RenderAsync(new RenderRequest()
            {
                Template = new Template()
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.Handlebars,
                    Content = html
                }
            }).GetAwaiter().GetResult();

            var bytes = new byte[report.Content.Length];
            report.Content.Read(bytes, 0, bytes.Length);
            return bytes;
            
        }
    }
}
