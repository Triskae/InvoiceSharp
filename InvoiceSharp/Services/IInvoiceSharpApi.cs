﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InvoiceSharp.Models;

namespace InvoiceSharp.Services
{
    public interface IInvoiceSharpApi : IInvoicerOptions
    {

    }

    public interface IInvoicerOptions : IInvoicerActions
    {
        /// <summary>
        /// Set a custom html color to personalize the document background
        /// </summary>
        IInvoicerOptions BackColor(string color);
        
        /// <summary>
        /// Set a custom html color to personalize the document text
        /// </summary>
        IInvoicerOptions TextColor(string color);

        /// <summary>
        /// Add a logo to the left corner of the document.
        /// </summary>
        IInvoicerOptions Image(string image, int width, int height);

        /// <summary>
        /// Set the title used on the document (e.g. 'invoice' or 'quote').
        /// </summary>
        /// <param name="title">Title. Default is 'Invoice'.</param>
        IInvoicerOptions Title(string title);

        /// <summary>
        /// A unique reference number for the order.
        /// </summary>
        /// <param name="reference">Reference (e.g. '123456-1'). If ommited the line while not be displayed.</param>
        IInvoicerOptions OrderReference(string reference);

        /// <summary>
        /// Set the document billing date
        /// </summary>
        /// <param name="date">Date, if ommited this is set to todays date.</param>
        IInvoicerOptions BillingDate(DateTime date);

        /// <summary>
        /// Set the due date.
        /// </summary>
        /// <param name="dueDate">Date, if ommited this is set to todays date + 14 days.</param>
        IInvoicerOptions PayedDate(DateTime dueDate);

        /// <summary>
        /// Set the payement status of the invoice.
        /// </summary>
        /// /// <param name="isUnpaid"></param>
        IInvoicerOptions IsUnpaid(bool isUnpaid);
        
        /// <summary>
        /// Set the payement status message for the invoice. Overrides the default message.
        /// </summary>
        /// /// <param name="message"></param>
        IInvoicerOptions UnpaidMessage(string message);
        
        /// <summary>
        /// Set the payement status message for the invoice. Overrides the default message.
        /// </summary>
        /// /// <param name="message"></param>
        IInvoicerOptions PaidMessage(string message);

        /// <summary>
        /// Set the company address
        /// </summary>
        IInvoicerOptions Company(Address address);
        
        /// <summary>
        /// Set the client address.
        /// </summary>
        IInvoicerOptions Client(Address address);
        
        /// <summary>
        /// Switch the position of the company address (default left).
        /// </summary>
        /// <param name="position">Orientation left or right.</param>
        IInvoicerOptions CompanyOrientation(PositionOption position);

        /// <summary>
        /// You can add titles and paragraphs to display information on the bottom area
        /// of the document such as payment details or shipping information.
        /// </summary>
        IInvoicerOptions Details(List<DetailRow> details);

        /// <summary>
        /// Set a footer, this would typically be a web url.
        /// </summary>
        IInvoicerOptions Footer(string title);

        /// <summary>
        /// Add a new product or service row to your document below the company and 
        /// client information, paging is automatic.
        /// </summary>
        IInvoicerOptions Items(List<ItemRow> items);

        /// <summary>
        /// Add a row below the products and services for calculations and totals.
        /// </summary>
        IInvoicerOptions Totals(List<TotalRow> totals);
    };

    public interface IInvoicerActions
    {
        /// <summary>
        /// Save the document with a password
        /// </summary>
        /// <param name="filename">Filename of the PDF that will be created</param>
        /// <param name="password">Leave null (default) or set a password</param>
        void Save(string filename = null, string password = null);

        Stream Get(string password = null);
    }
}
