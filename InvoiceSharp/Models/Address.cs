using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSharp.Models
{
    public class Address
    {
        public string Title { get; set; }
        public string[] AddressLines { get; set; }
        public string VatNumber { get; set; }
        public string CompanyNumber { get; set; } // TODO: Is this needed here as we now have legal text

        public string[] LegalTextLines { get; set; }

        public bool HasCompanyNumber { get { return !string.IsNullOrEmpty(CompanyNumber); } }
        public bool HasVatNumber { get { return !string.IsNullOrEmpty(VatNumber); } }
        public bool HasLegalTextLines { get { return LegalTextLines != null; } }

        public static Address Make(string title, string[] address, string company = null, string vat = null, string[] legalTextLines = null)
        {
            return new Address
            {
                Title = title,
                AddressLines = address,
                CompanyNumber = company,
                VatNumber = vat,
                LegalTextLines = legalTextLines
            };
        }
    }
}
