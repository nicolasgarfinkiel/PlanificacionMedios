using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace Irsa.PDM.Infrastructure
{
    public class PdfGenerator
    {
        private readonly string _html;
        private const string HeaderTagStart = "<PDF_HEADER>";
        private const string HeaderTagEnd = "</PDF_HEADER>";

        public PdfGenerator(string html)
        {
            _html = html;
        }
        public byte[] Generate()
        {
            var result = default(byte[]);

            Validate();
            var indexStart = _html.IndexOf(HeaderTagStart);
            var indexEnd = _html.IndexOf(HeaderTagEnd);

            var headerContent = _html.Substring(indexStart, indexEnd - indexStart);
            var eventsHelper = new PageEventHelper(headerContent);

            using (var ms = new MemoryStream())
            using (var document = new Document(PageSize.A4, 0, 0, 10, 20))
            {
                var writer = PdfWriter.GetInstance(document, ms);
                writer.PageEvent = eventsHelper;

                using (var reader = new StringReader(_html))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);
                    document.Close();
                    result = ms.GetBuffer();
                }
            }

            return result;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(_html))
            {
                throw new Exception("Document Empty");
            }

            if (!_html.Contains(HeaderTagStart) || !_html.Contains(HeaderTagEnd))
            {
                throw new Exception(string.Format("Invalid document format. {0} tag not found", HeaderTagStart));
            }
        }
    }


    public class PageEventHelper : PdfPageEventHelper
    {
        private ElementList _header;
        public PageEventHelper(string headerHtml)
        {
            _header = XMLWorkerHelper.ParseToElementList(headerHtml, null);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            var ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(36, 832, 559, 810));

            foreach (var e in _header)
            {
                ct.AddElement(e);
            }

            ct.Go();
        }
    }
}
