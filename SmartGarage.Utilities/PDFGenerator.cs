using DinkToPdf;

namespace SmartGarage.Utilities
{
    internal class PDFGenerator
    {
        private readonly SynchronizedConverter _converter;

        public PDFGenerator()
        {
            _converter = new SynchronizedConverter(new PdfTools());
        }

        public byte[] ConvertHtmlToPdf(string htmlContent)
        {
            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    },
                        Objects = {
                        new ObjectSettings
                        {
                            PagesCount = true,
                            HtmlContent = htmlContent,
                        }
                    }
                };

            return _converter.Convert(document);
        }
    }
}
