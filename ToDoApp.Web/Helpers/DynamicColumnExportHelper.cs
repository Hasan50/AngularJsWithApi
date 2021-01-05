using ToDoApp.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ToDoApp.Web.Helpers
{

    public class DynamicColumnExportHelper
    {
        public static void DynamicColumnWriteHtmlTable<T>(IEnumerable<T> data, IEnumerable<DynamicColumnModel> headerList, TextWriter output, string title)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    var table = DynamicColumnHtmlTableToExcel(data, headerList, htw, title);
                    output.Write(sw.ToString());
                }
            }

        }
        private static Table DynamicColumnHtmlTableToExcel<T>(IEnumerable<T> data, IEnumerable<DynamicColumnModel> headerList, HtmlTextWriter htw, string title)
        {
            Table table = new Table();
            TableRow row = new TableRow();

            foreach (var h in headerList)
            {
                if (h.IsExcludeInExle)
                    continue;

                TableHeaderCell hcell = new TableHeaderCell();
                hcell.Text = h.label;
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
                foreach (var c in headerList)
                {
                    if (c.IsExcludeInExle)
                        continue;

                    var property = props.Find(c.name, false);

                    TableCell cell = new TableCell();

                    //if (c.IsStringInExle)
                    //    cell.Attributes.CssStyle.Add("mso-number-format", "\\@");

                    if (!string.IsNullOrEmpty(property.Description))
                        cell.Attributes.CssStyle.Add("mso-number-format", "\\@");
                    

                    cell.Text = property.Converter.ConvertToString(property.GetValue(item));
                    cell.BorderStyle = BorderStyle.Solid;
                    cell.BorderWidth = Unit.Pixel(1);
                    cell.BorderColor = System.Drawing.Color.Gray;
                    row.Cells.Add(cell);

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