using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;

namespace Irsa.PDM.Infrastructure.ActionResults
{
    public class ExcelResult : ActionResult
    {
        public string FileName { get; set; }
        public IList<object> Data { get; set; }
        public ExcelPackage ExcelPackage { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Buffer = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            context.HttpContext.Response.ContentType = "application/vnd.ms-excel";

            if (ExcelPackage == null)
                context.HttpContext.Response.Write(GetDataFormated().ToString());
            else
                context.HttpContext.Response.BinaryWrite(ExcelPackage.GetAsByteArray());

            context.HttpContext.Response.End();
        }

        private StringWriter GetDataFormated()
        {
            var stringWriter = new StringWriter();
            var htmlWrite = new HtmlTextWriter(stringWriter);
            var dataGrid = new GridView { DataSource = GetDataTable() };
            dataGrid.DataBind();
            dataGrid.RenderControl(htmlWrite);

            return stringWriter;
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            Type t;
            PropertyInfo[] pi;

            if (Data.Count == 0)
                throw new Exception("");

            t = Data[0].GetType();
            pi = t.GetProperties();

            foreach (var p in pi)
                dataTable.Columns.Add(new DataColumn(p.Name));


            foreach (var o in Data)
            {
                t = o.GetType();
                pi = t.GetProperties();
                var row = dataTable.NewRow();

                foreach (var p in pi)
                    row[p.Name] = p.GetValue(o, null);

                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}