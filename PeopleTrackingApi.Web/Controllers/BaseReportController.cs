using Microsoft.Reporting.WebForms;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using PeopleTrackingApi.Web.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PeopleTrackingApi.Web.Controllers
{
    public class BaseReportController : Controller
    {
        protected readonly UserSessionModel _userInfo;
        public BaseReportController()
        {
            _userInfo = CommonUtility.GetCurrentUser();
        }

        protected const string CompanyAddress = "";
        protected const string CompanyName = "";
        protected const string PdfDeviceInfoA4Size =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.25in</PageWidth>" +
                "  <PageHeight>11.6in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
        protected const string PdfDeviceInfoA3Size =
               "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>16.54in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
        protected const string PdfDeviceInfoLandscape =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

        //
        // GET: /BaseReport/
        protected ActionResult ViewReportFormat(LocalReport localReport)
        {
            return GeneratePdfReport(localReport, PdfDeviceInfoA4Size);
        }

        protected ActionResult ViewReportFormatAsA3(LocalReport localReport)
        {
            return GeneratePdfReport(localReport, PdfDeviceInfoA3Size);
        }

        protected ActionResult ViewReportFormatLandScape(LocalReport localReport)
        {
            return GeneratePdfReport(localReport, PdfDeviceInfoLandscape);
        }


        protected void ExportToCsv(string reportName, StringBuilder sb)
        {
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition",string.Format("attachment;filename={0}.CSV",reportName));
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }
        private ActionResult GeneratePdfReport(LocalReport localReport,string deviceInfo)
        {
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report             
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

        protected void ExportToExcel<T>(List<T> list, string reportName)
        {
            var dataGrid = new GridView();
            if (list != null && list.Count > 0)
            {
                dataGrid.DataSource = list.AsQueryable();
                dataGrid.DataBind();
            }
            
            var fileName = "filename=" + reportName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; " + fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            dataGrid.AllowPaging = false;
            dataGrid.RenderControl(htw);
            string style = @"<style> td { mso-number-format:\@;} </style>";
            Response.Write(style);
            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        protected void ExportToExcelAsFormated<T>(List<T> list, string reportName, string title)
        {
            var fileName = "filename=" + reportName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; " + fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            ExportToExcelHelper.WriteHtmlTable(list, Response.Output, title);
            Response.End();
        }
       
        protected void ExportToExcelAsFormated<T>(List<T> list, List<string> headerList,string reportName, string title)
        {
            var fileName = "filename=" + reportName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; " + fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            ExportToExcelHelper.WriteHtmlTableAsDynamicHeader(list, headerList, Response.Output, title);
            Response.End();
        }

       
        protected void SaveExcelFile<T>(IEnumerable<T> data, string locationPath, string fileName, string title)
        {
            var filePath = Path.Combine(locationPath, fileName);
            var streamWriter = new StreamWriter(filePath);
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    var table =ExportToExcelHelper.GetHtmlTableIfExcel(data, htw, title);
                    streamWriter.Write(sw.ToString());
                }
            }
            streamWriter.Close();
        }
        protected void DynamicColumnExportToExcel<T>(List<T> list, IEnumerable<DynamicColumnModel> headerList, string reportName, string title)
        {
            var fileName = "filename=" + reportName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; " + fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            DynamicColumnExportHelper.DynamicColumnWriteHtmlTable(list, headerList, Response.Output,title);
            Response.End();
        }
       
    }
}