using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PeopleTrackingApi.Web.Helpers
{
    public class ExportToExcelHelper
    {
        
        public static void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output, string title)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    var table = GetHtmlTableIfExcel(data, htw, title);
                    output.Write(sw.ToString());
                }
            }

        }

        public static void WriteHtmlTableAsDynamicHeader<T>(IEnumerable<T> data, List<string> headerList, TextWriter output, string title)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    var table = GetHtmlDynamicTableIfExcel(data, headerList, htw, title);
                    output.Write(sw.ToString());
                }
            }

        }

        public static Table GetHtmlTableIfExcel<T>(IEnumerable<T> data, HtmlTextWriter htw, string title)
        {
            Table table = new Table();
            TableRow row = new TableRow();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in props)
            {
                if (prop.IsBrowsable)
                {
                    TableHeaderCell hcell = new TableHeaderCell();
                    var dName = prop.DisplayName;
                    hcell.Text = string.IsNullOrEmpty(dName) ? prop.Name : dName;
                    hcell.BackColor = System.Drawing.Color.LightGreen;
                    hcell.BorderStyle = BorderStyle.Solid;
                    hcell.BorderWidth = Unit.Pixel(1);
                    hcell.BorderColor = System.Drawing.Color.Gray;
                    row.Cells.Add(hcell);
                }
            }
            table.Rows.Add(row);


            foreach (T item in data)
            {
                row = new TableRow();
                foreach (PropertyDescriptor prop in props)
                {
                    if (prop.IsBrowsable)
                    {
                        TableCell cell = new TableCell();
                        if (!string.IsNullOrEmpty(prop.Description))
                        {
                            cell.Attributes.CssStyle.Add("mso-number-format", "\\@");
                        }
                        cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                        cell.BorderStyle = BorderStyle.Solid;
                        cell.BorderWidth = Unit.Pixel(1);
                        cell.BorderColor = System.Drawing.Color.Gray;
                        row.Cells.Add(cell);
                    }
                    table.Rows.Add(row);
                }
            }
            if (!string.IsNullOrEmpty(title))
            {
                htw.Write(string.Format("<div style='text-align:center;font-size:25px;margin-top:20px;margin-bottom:20px;'>{0}</div>", title));
            }

            table.RenderControl(htw);
            return table;
        }

        private static Table GetHtmlDynamicTableIfExcel<T>(IEnumerable<T> data, List<string> headerList, HtmlTextWriter htw, string title)
        {
            Table table = new Table();
            TableRow row = new TableRow();

            foreach (var h in headerList)
            {
                TableHeaderCell hcell = new TableHeaderCell();
                hcell.Text = h;
                hcell.BackColor = System.Drawing.Color.LightGreen;
                hcell.BorderStyle = BorderStyle.Solid;
                hcell.BorderWidth = Unit.Pixel(1);
                hcell.BorderColor = System.Drawing.Color.Gray;
                row.Cells.Add(hcell);
            }
            table.Rows.Add(row);

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (T item in data)
            {
                row = new TableRow();
                foreach (PropertyDescriptor prop in props)
                {
                    if (prop.IsBrowsable)
                    {
                        TableCell cell = new TableCell();
                        if (!string.IsNullOrEmpty(prop.Description))
                        {
                            cell.Attributes.CssStyle.Add("mso-number-format", "\\@");
                        }
                        cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                        cell.BorderStyle = BorderStyle.Solid;
                        cell.BorderWidth = Unit.Pixel(1);
                        cell.BorderColor = System.Drawing.Color.Gray;
                        row.Cells.Add(cell);
                    }
                    table.Rows.Add(row);
                }
            }
            if (!string.IsNullOrEmpty(title))
            {
                htw.Write(string.Format("<div style='text-align:left;font-size:25px;margin-top:20px;margin-bottom:20px;'>{0}</div>", title));
            }

            table.RenderControl(htw);
            return table;
        }


    }
}