using iText.Layout;
using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

using SmartGarage.Data.Models;

namespace SmartGarage.Utilities
{
    public class PDFGenerator
    {
        public byte[] GeneratePdf(Visit visit)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var doc = new Document(pdf);

                //add title

                {
                    var title = new Paragraph()
                        .SetFontSize(30)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBold()
                        .SetFontColor(ColorConstants.LIGHT_GRAY);

                    title.Add("Smart Garage");

                    doc.Add(title);
                }

                // add license plate number and date

                {
                    var header = new Paragraph()
                        .SetFontSize(20)
                        .SetMarginTop(30);

                    header.Add($"{visit.Vehicle.LicensePlateNumber} {new string('\t', 14)} {DateTime.Now.ToShortDateString()}");

                    doc.Add(header);
                }

                // add repair activities

                {
                    var table = new Table(2)
                        .SetMarginTop(30);

                    table.AddCell(CreateCell("Service Type", true).SetFontSize(16));
                    table.AddCell(CreateCell("Price", false).SetFontSize(16));

                    foreach (var activity in visit.RepairActivities)
                    {
                        table.AddCell(CreateCell(activity.RepairActivityType.Name, true));
                        table.AddCell(CreateCell($"+{activity.Price:f2} BGN", false));
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph(new string('_', 78)));
                }

                //add total price

                {
                    var table = new Table(2)
                        .SetMarginTop(10);

                    var price = visit.RepairActivities.Sum(x => x.Price);

                    table.AddCell(CreateCell("Total Price:", true));
                    table.AddCell(CreateCell($"{price:f2} BGN", false));

                    table.AddCell(CreateCell("Discount:", true));
                    table.AddCell(CreateCell($"{visit.DiscountPercentage}% ({price * visit.DiscountPercentage / 100:f2} BGN)", false));

                    table.AddCell(CreateCell("Total Price After Discount:", true));
                    table.AddCell(CreateCell($"{price - price * visit.DiscountPercentage / 100:f2} BGN", false));

                    doc.Add(table);
                }

                // add signature

                {
                    var signature = new Paragraph();

                    signature
                        .SetMarginTop(50)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER);

                    signature.Add($"Thank you for your trust!{Environment.NewLine}");
                    signature.Add($"+359 879 569 542{Environment.NewLine}");
                    signature.Add($"Ulica ulichna 11{Environment.NewLine}");

                    doc.Add(signature);
                }

                doc.Close();

                return stream.ToArray();
            }
        }

        private static Cell CreateCell(string content, bool isLeft)
        {
            var cell = new Cell()
                .Add(new Paragraph(content))
                .SetBorder(Border.NO_BORDER)
                .SetItalic()
                .SetWidth(300);

            if (isLeft)
            {
                cell
                    .SetTextAlignment(TextAlignment.LEFT);
            }
            else
            {
                cell
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBold();
            }

            return cell;
        }
    }
}
