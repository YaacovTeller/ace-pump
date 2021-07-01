using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using System.Data;
using System;

namespace AcePump.Rdlc.Builder
{
    public  class RdlcBuilder
    {
        public string SaveAsName;

        private LocalReport _Report;
        private LocalReport Report
        {
            get
            {
                if (_Report == null) _Report = new LocalReport();

                return _Report;
            }
        }

        private static Func<string, string> _RdlcPathMapper;
        public static void SetPathMapper(Func<string, string> rdlcPathMapper) {
            _RdlcPathMapper = rdlcPathMapper;
        }

        public void SetReportPath(string rdlcPath)
        {
            Report.ReportPath =  _RdlcPathMapper(rdlcPath);
        }

        public void SetEnableExternalImages(bool set)
        {
            Report.EnableExternalImages = set;
        }

        public void LoadReportFromStream(Stream reportDefinition)
        {
            Report.LoadReportDefinition(reportDefinition);
        }

        public void LoadReportFromAssembly(Assembly assembly, string resourceName)
        {
            Stream reportStream = assembly.GetManifestResourceStream(resourceName);

            LoadReportFromStream(reportStream);
        }

        public void LoadReportFromAssembly(string resourceName)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            LoadReportFromAssembly(callingAssembly, resourceName);
        }

        public void AddDataSetFromObject<TEntity>(string name, TEntity item)
        {
            DataTable asDataTable = DataTableConverter.FromObject(item);
            ReportDataSource dataSource = new ReportDataSource(name, asDataTable);

            AddDataSet(dataSource);
        }

        public void AddDataSet<TEntity>(string name, IEnumerable<TEntity> data)
        {
            DataTable asDataTable = DataTableConverter.FromEnumerable(data);
            ReportDataSource dataSource = new ReportDataSource(name, asDataTable);

            AddDataSet(dataSource);
        }

        public void AddDataSet(ReportDataSource dataSource)
        {
            Report.DataSources.Add(dataSource);
        }

        public void AddParameter(string name, string value)
        {
            Report.SetParameters(new ReportParameter(name, value));
        }

        public LocalReport GetReport()
        {
            return Report;
        }

        //public FileContentResult GetFileContentResult(string fileDownloadName = null)
        //{
        //    GeneratePdf();

        //    var result = new FileContentResult(PdfBytes, MimeType);
        //    result.FileDownloadName = fileDownloadName;

        //    return result;
        //}

        //public HttpResponseMessage GetHttpResponseMessageResult(string fileDownloadName = null)
        //{
        //    GeneratePdf();

        //    MemoryStream MemoryStream = new MemoryStream(PdfBytes);
        //    var Buffer = MemoryStream.ToArray();
        //    int contentLength = Buffer.Length;

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    response.Content = new StreamContent(new MemoryStream(Buffer));
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //    response.Content.Headers.ContentLength = contentLength;

        //    ContentDispositionHeaderValue ContentDisposition = null;
        //    if ((ContentDispositionHeaderValue.TryParse("inline; filename=" + fileDownloadName + ".pdf", out ContentDisposition)))
        //        response.Content.Headers.ContentDisposition = ContentDisposition;

        //    return response;
        //}
    }
}
