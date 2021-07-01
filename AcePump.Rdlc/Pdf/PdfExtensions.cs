using System;
using System.IO;
using AcePump.Rdlc.Builder;
using Microsoft.Reporting.WebForms;

namespace AcePump.Rdlc.Pdf
{
    public static class PdfExtensions
    {
        private static MemoryStream reportStream;

        private static Stream CreateStream(string name, string fileNameExtension, System.Text.Encoding encoding, string mimeType, bool willSeek)
        {
            reportStream = new MemoryStream();
            return reportStream;
        }

        public static MemoryStream CreatePdfStream(this RdlcBuilder builder)
            
        {
            LocalReport report = builder.GetReport();

            report.Refresh();

            string deviceInfo = null;
            Warning[] outWarnings = null;
                      
            report.Render("PDF", deviceInfo, CreateStream, out outWarnings);
            reportStream.Position = 0;

            return reportStream;
        }
    }
}
