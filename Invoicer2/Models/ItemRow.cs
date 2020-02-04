using System;
using System.Collections.Generic;

namespace Invoicer2.Models
{
    public class ItemRow
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal VAT { get; set; }
        public decimal Price { get; set; }
        public string Discount { get; set; }
        public decimal Total { get; set; }

        public bool HasDiscount
        {
            get
            {
                return (!string.IsNullOrEmpty(Discount));
            }
        }

        public static ItemRow Make(string name, string description, decimal quantity, decimal vat, decimal price, decimal total)
        {
            return Make(name, description, quantity, vat, price, "", total);
        }

        public static ItemRow Make(string name, string description, decimal quantity, decimal vat, decimal price, string discount, decimal total)
        {
            return new ItemRow()
            {
                Name = name,
                Description = description,
                Quantity = quantity,
                VAT = vat,
                Price = price,
                Discount = discount,
                Total = total,
            };
        }
    }
}