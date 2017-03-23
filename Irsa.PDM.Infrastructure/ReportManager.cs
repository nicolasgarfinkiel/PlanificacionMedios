using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Reporting.WebForms;

namespace Irsa.PDM.Infrastructure
{
    public static class ReportManager
    {
        public static byte[] Render(string report, DataSet dataSet, string format, IList<ReportParameter> parameters = null)
        {
            string mimeType, encoding, extension;
            string[] streamids;
            Warning[] warnings;

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;


            using (var rdlcSr = new StreamReader(string.Format(@"{0}\Reports\{1}", AppDomain.CurrentDomain.BaseDirectory, report)))
            {
                reportViewer.LocalReport.LoadReportDefinition(rdlcSr);
                reportViewer.LocalReport.Refresh();
            }

            dataSet.Tables.Cast<DataTable>().ToList()
                .ForEach(t => reportViewer.LocalReport.DataSources.Add(new ReportDataSource(t.TableName, t)));

            if (parameters != null)
            {
                reportViewer.LocalReport.SetParameters(parameters);
            }

            return reportViewer.LocalReport.Render(format, "", out mimeType, out encoding, out extension, out streamids, out warnings);
        }
    }
}
