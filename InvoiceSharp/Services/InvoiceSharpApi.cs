using InvoiceSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using InvoiceSharp.Models;
using InvoiceSharp.Services.Impl;

namespace InvoiceSharp.Services
{
    public class InvoiceSharpApi : IInvoiceSharpApi
    {
        public Invoice Invoice { get; protected set; }
        
        public static string DefaultReference => "";

        public InvoiceSharpApi(
            SizeOption size = SizeOption.A4,
            OrientationOption orientation = OrientationOption.Portrait,
            string currency = "€"
            )
        {
            Invoice = new Invoice();
            Invoice.Title = "Facture";
            Invoice.PageSize = size;
            Invoice.PageOrientation = orientation;
            Invoice.Currency = currency;
            Invoice.InvoiceDate = DateTime.Now;
            Invoice.PayedDate = Invoice.InvoiceDate.AddDays(14);
            Invoice.OrderReference = DefaultReference;
        }

        public IInvoicerOptions BackColor(string color)
        {
            Invoice.BackColor = color;
            return this;
        }

        public IInvoicerOptions TextColor(string color)
        {
            Invoice.TextColor = color;
            return this;
        }

        public IInvoicerOptions Image(string image, int width, int height)
        {
            Invoice.Image = image;
            Invoice.ImageSize = new Size(width, height);
            return this;
        }


        public IInvoicerOptions Title(string title)
        {
            Invoice.Title = title;
            return this;
        }

        public IInvoicerOptions OrderReference(string reference)
        {
            Invoice.OrderReference = reference;
            return this;
        }

        public IInvoicerOptions BillingDate(DateTime date)
        {
            Invoice.InvoiceDate = date;
            return this;
        }

        public IInvoicerOptions PayedDate(DateTime dueDate)
        {
            Invoice.PayedDate = dueDate;
            return this;
        }

        public IInvoicerOptions Company(Address address)
        {
            Invoice.Company = address;
            return this;
        }

        public IInvoicerOptions Client(Address address)
        {
            Invoice.Client = address;
            return this;
        }

        public IInvoicerOptions CompanyOrientation(PositionOption position)
        {
            Invoice.CompanyOrientation = position;
            return this;
        }

        public IInvoicerOptions Details(List<DetailRow> details)
        {
            Invoice.Details = details;
            return this;
        }

        public IInvoicerOptions Footer(string title)
        {
            Invoice.Footer = title;
            return this;
        }

        public IInvoicerOptions Items(List<ItemRow> items)
        {
            Invoice.Items = items;
            return this;
        }

        public IInvoicerOptions Totals(List<TotalRow> totals)
        {
            Invoice.Totals = totals;
            return this;
        }

        public void Save(string filename, string password = null)
        {
            if (filename == null || !System.IO.Path.HasExtension(filename))
                filename = System.IO.Path.ChangeExtension(Invoice.OrderReference, "pdf");
            new PdfInvoice(Invoice).Save(filename, password);
        }

        public Stream Get(string password = null)
        {
            return new PdfInvoice(Invoice).Get(password);
        }
    }
}
