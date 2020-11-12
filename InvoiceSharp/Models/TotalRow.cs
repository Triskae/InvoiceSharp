using System;
using System.Collections.Generic;

namespace InvoiceSharp.Models
{
    public class TotalRow
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public bool Inverse { get; set; }
        public bool NegativeAmount { get; set; }

        public static TotalRow Make(string name, decimal value, bool inverse = false, bool negativeAmount = false)
        {
            return new TotalRow()
            {
                Name = name,
                Value = value,
                Inverse = inverse,
                NegativeAmount = negativeAmount
            };
        }
    }
}