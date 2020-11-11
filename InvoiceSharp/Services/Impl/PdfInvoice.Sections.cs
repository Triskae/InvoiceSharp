using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Linq;
using InvoiceSharp.Helpers;
using InvoiceSharp.Models;

namespace InvoiceSharp.Services.Impl
{
    public partial class PdfInvoice
    {
        private void HeaderSection()
        {
            HeaderFooter header = Pdf.LastSection.Headers.Primary;

            Table table = header.AddTable();
            double thirdWidth = Pdf.PageWidth() / 3;

            table.AddColumn(ParagraphAlignment.Left, thirdWidth * 2);
            table.AddColumn();

            Row row = table.AddRow();

            if (!string.IsNullOrEmpty(Invoice.Image))
            {
                Image image = row.Cells[0].AddImage(Invoice.Image);
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                image.Height = Invoice.ImageSize.Height;
                image.Width = Invoice.ImageSize.Width;
            }

            TextFrame frame = row.Cells[1].AddTextFrame();

            Table subTable = frame.AddTable();
            subTable.AddColumn(thirdWidth / 2);
            subTable.AddColumn(thirdWidth / 2);

            row = subTable.AddRow();
            row.Cells[0].MergeRight = 1;
            row.Cells[0].AddParagraph(Invoice.Title, ParagraphAlignment.Right, "H1-20");

            row = subTable.AddRow();
            if (!String.IsNullOrEmpty(Invoice.OrderReference))
            {
                row.Cells[0].AddParagraph("COMMANDE N°:", ParagraphAlignment.Left, "H2-9B-Color");
                row.Cells[1].AddParagraph(Invoice.OrderReference, ParagraphAlignment.Right, "H2-9");
            }
            row.Cells[0].AddParagraph("EDITÉE LE:", ParagraphAlignment.Left, "H2-9B-Color");
            row.Cells[1].AddParagraph(Invoice.InvoiceDate.ToString("dd/MM/yyyy"), ParagraphAlignment.Right, "H2-9");
            row.Cells[0].AddParagraph("PAYÉE LE:", ParagraphAlignment.Left, "H2-9B-Color");
            row.Cells[1].AddParagraph(Invoice.PayedDate.ToString("dd/MM/yyyy"), ParagraphAlignment.Right, "H2-9");
        }

        public void FooterSection()
        {
            HeaderFooter footer = Pdf.LastSection.Footers.Primary;

            Table table = footer.AddTable();
            table.AddColumn(footer.Section.PageWidth());
            table.AddColumn(footer.Section.PageWidth()/10);
            Row row = table.AddRow();
            
            if (Invoice.Company.HasLegalTextLines)
            {
                foreach (var line in Invoice.Company.LegalTextLines)
                {
                    row.Cells[0].AddParagraph(line, ParagraphAlignment.Center, "H2-9");
                }
            }

            row.Cells[1].AddParagraph();
            Paragraph info = row.Cells[1].AddParagraph();
            info.Format.Alignment = ParagraphAlignment.Right;
            info.Style = "H2-9";
            info.AddPageField();
            info.AddText(" / ");
            info.AddNumPagesField();
        }

        private void AddressSection()
        {
            Section section = Pdf.LastSection;

            Address leftAddress = Invoice.Company;
            Address rightAddress = Invoice.Client;

            if (Invoice.CompanyOrientation == PositionOption.Right)
                Utils.Swap<Address>(ref leftAddress, ref rightAddress);

            Table table = section.AddTable();
            table.AddColumn(ParagraphAlignment.Left, section.Document.PageWidth() / 2 - 10);
            table.AddColumn(ParagraphAlignment.Center, Unit.FromPoint(20));
            table.AddColumn(ParagraphAlignment.Left, section.Document.PageWidth() / 2 - 10);

            Row row = table.AddRow();
            row.Style = "H2-10B-Color";
            row.Shading.Color = Colors.White;

            row.Cells[0].AddParagraph(leftAddress.Title, ParagraphAlignment.Left);
            row.Cells[0].Format.Borders.Bottom = BorderLine;
            row.Cells[2].AddParagraph(rightAddress.Title, ParagraphAlignment.Left);
            row.Cells[2].Format.Borders.Bottom = BorderLine;

            row = table.AddRow();
            AddressCell(row.Cells[0], leftAddress.AddressLines);
            AddressCell(row.Cells[2], rightAddress.AddressLines);

            row = table.AddRow();
        }

        private void AddressCell(Cell cell, string[] address)
        {
            foreach (string line in address)
            {
                Paragraph name = cell.AddParagraph();
                if (line == address[0])
                    name.AddFormattedText(line, "H2-10B");
                else
                    name.AddFormattedText(line, "H2-9-Grey");
            }
        }

        private void BillingSection()
        {
            Section section = Pdf.LastSection;

            Table table = section.AddTable();

            double width = section.PageWidth();

            //double productWidth = Unit.FromPoint(150);
            //double numericWidth = (width - productWidth) / this.COLUMN_TOTAL;

            double numericWidth = Unit.FromPoint(70);
            double productWidth = (width - (numericWidth * this.COLUMN_TOTAL));
            

            table.AddColumn(productWidth);

            for(int i = 0; i <= this.COLUMN_TOTAL - 1; i++)
            {
                table.AddColumn(ParagraphAlignment.Center, numericWidth);
            }
            
            BillingHeader(table);

            foreach (ItemRow item in Invoice.Items)
            {
                BillingRow(table, item);
            }

            if (Invoice.Totals != null)
            {
                foreach (TotalRow total in Invoice.Totals)
                {
                    BillingTotal(table, total);
                }
            }
            table.AddRow();
        }

        private void BillingHeader(Table table)
        {
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Style = "H2-10B-Color";
            row.Shading.Color = Colors.White;
            row.TopPadding = 10;
            row.Borders.Bottom = BorderLine;

            row.Cells[COLUMN_PRODUCT].AddParagraph("PRODUIT", ParagraphAlignment.Left); // TODO: Was PRODUCT - can we have a setting for Product/Service invoices?
            row.Cells[COLUMN_QTY].AddParagraph("QTÉ", ParagraphAlignment.Right);
            row.Cells[COLUMN_UNITPRICE].AddParagraph("PRIX HT", ParagraphAlignment.Right);
            row.Cells[COLUMN_TOTAL].AddParagraph("TOTAL", ParagraphAlignment.Right);

            if (Invoice.Company.HasVatNumber)
            {
                row.Cells[COLUMN_VATPERCENT].AddParagraph("TVA", ParagraphAlignment.Right);
            }

            if (Invoice.HasDiscount)
            {
                row.Cells[COLUMN_DISCOUNT].AddParagraph("REMISE", ParagraphAlignment.Right);
            }

            
        }

        private void BillingRow(Table table, ItemRow item)
        {
            Row row = table.AddRow();
            row.Style = "TableRow";
            row.Shading.Color = MigraDocHelpers.BackColorFromHtml(Invoice.BackColor);

            Cell cell = row.Cells[COLUMN_PRODUCT];
            cell.AddParagraph(item.Name, ParagraphAlignment.Left, "H2-9B");
            if (!String.IsNullOrEmpty(item.Description))
            {
                cell.AddParagraph(item.Description, ParagraphAlignment.Left, "H2-9-Grey");
            }

            cell = row.Cells[COLUMN_QTY];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph(item.Quantity.ToCurrency(), ParagraphAlignment.Right, "H2-9");

            cell = row.Cells[COLUMN_UNITPRICE];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph(item.Price.ToCurrency(Invoice.Currency), ParagraphAlignment.Right, "H2-9");

            cell = row.Cells[COLUMN_TOTAL];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph(item.Total.ToCurrency(Invoice.Currency), ParagraphAlignment.Right, "H2-9");

            if (Invoice.Company.HasVatNumber)
            {
                cell = row.Cells[COLUMN_VATPERCENT];
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.AddParagraph(item.VAT.ToCurrency(), ParagraphAlignment.Right, "H2-9");
            }

            if (Invoice.HasDiscount)
            {
                cell = row.Cells[COLUMN_DISCOUNT];
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.AddParagraph(item.Discount, ParagraphAlignment.Right, "H2-9");
            }
        }

        private void BillingTotal(Table table, TotalRow total)
        {
            Row row = table.AddRow();
            row.Style = "TableRow";

            string font; Color shading;
            if (total.Inverse == true)
            {
                font = "H2-9B-Inverse";
                shading = MigraDocHelpers.TextColorFromHtml(Invoice.TextColor);
            }
            else
            {
                font = "H2-9B";
                shading = MigraDocHelpers.BackColorFromHtml(Invoice.BackColor);
            }

            Cell cell = row.Cells[Invoice.HasDiscount ? COLUMN_DISCOUNT : COLUMN_UNITPRICE];
            cell.Shading.Color = shading;
            cell.AddParagraph(total.Name, ParagraphAlignment.Left, font);

            cell = row.Cells[COLUMN_TOTAL];
            cell.Shading.Color = shading;
            cell.AddParagraph(total.Value.ToCurrency(Invoice.Currency), ParagraphAlignment.Right, font);
        }

        private void PaymentSection()
        {
            Section section = Pdf.LastSection;

            Table table = section.AddTable();
            table.AddColumn(Unit.FromPoint(section.Document.PageWidth()));
            Row row = table.AddRow();

            if (Invoice.Details != null && Invoice.Details.Count > 0)
            {
                foreach (DetailRow detail in Invoice.Details)
                {
                    row.Cells[0].AddParagraph(detail.Title, ParagraphAlignment.Left, "H2-9B-Color");
                    row.Cells[0].Borders.Bottom = BorderLine;

                    row = table.AddRow();
                    TextFrame frame = null;
                    foreach (string line in detail.Paragraphs)
                    {
                        Paragraph name = row.Cells[0].AddParagraph();
                        name.AddFormattedText(line, "H2-9-Grey");
                    }
                }
            }
        }
    }
}
