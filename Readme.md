# InvoiceSharp

Wrapper arround Migradoc and PDFSharp to generate pdf invoices easy peasy.
Based on Invoicer Thanks a lot ! 
Invoicer was heavily used and modified to be more flexible and (for the moment) translated in french (maybe more to come ?)

### Usage

		byte[] image = LoadImage("test.jpg");
            	string imageFilename = MigraDocFilenameFromByteArray(image);

            new InvoicerApi(SizeOption.A4, OrientationOption.Portrait, "€")
                .TextColor("#057a55")
                .BackColor("#F7FAFC")
                .Image(imageFilename, 70, 70)
                .Company(Address.Make(
                    "FROM",
                    new string[]
                    {
                        "Test Limited",
                        "Test House",
                        "Rue de la paix",
                        "Paris",
                        "CEDEX 15"
                    },
                    "1471587",
                    "569953277",
                    new string[]
                    {
                        "Vodafone Limited. Registered in England and Wales No. 1471587.",
                        "Registered office: Vodafone House, The Connection, Newbury, Berkshire RG14 2FN."
                    }))
                .Client(Address.Make("INVOICE TO",
                    new string[] {"Isabella Marsh", "Overton Circle", "Little Welland", "Worcester", "WR## 2DJ"}))
                .Items(GenerateFakeDate())
                .Totals(new List<TotalRow>
                {
                    TotalRow.Make("Sous Total", (decimal) 631.99, true),
                    TotalRow.Make("Total TVA 20%", (decimal) 20.99, true),
                    TotalRow.Make("Total TVA 10%", (decimal) 12.99, true),
                    TotalRow.Make("Grand Total", (decimal) 800.99, true),
                })
                .Details(new List<DetailRow>
                {
                    DetailRow.Make("NOTES", "Payé par Stripe", "",
                        "N'hésitez pas à nous contacter pour toutes questions ou toutes nouvelles commandes",
                        "", "Merci et à très vite !")
                })
                .BillingDate(DateTime.Now)
                .PayedDate(DateTime.Now)
                .Save("Invoice.pdf");

[![sample.png](https://i.postimg.cc/gcNvzN31/sample.png)](https://postimg.cc/dDZ7nRH9)
		
### References
* [PDFsharp/MigraDoc](http://pdfsharp.com)  
* [HTML Color Picker](http://www.w3schools.com/tags/ref_colorpicker.asp)
* [Original work](https://github.com/simonray/Invoicer)
