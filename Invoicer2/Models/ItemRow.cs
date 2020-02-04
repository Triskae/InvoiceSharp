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

        /// <summary>
        /// Makes an ItemRow without VAT or discount.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public static ItemRow Make(string name, string description, decimal quantity, decimal price, decimal total)
        {
            return Make(name, description, quantity, 0, price, "", total);
        }

        /// <summary>
        /// Makes an ItemRow without VAT and with discount.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        /// <param name="discount">The discount.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public static ItemRow Make(string name, string description, decimal quantity, decimal price, string discount, decimal total)
        {
            return Make(name, description, quantity, 0, price, discount, total);
        }

        /// <summary>
        /// Makes an ItemRow with VAT.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="vat">The vat.</param>
        /// <param name="price">The price.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public static ItemRow Make(string name, string description, decimal quantity, decimal vat, decimal price, decimal total)
        {
            return Make(name, description, quantity, vat, price, "", total);
        }

        /// <summary>
        /// Makes an ItemRow with VAT and discount.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="vat">The vat.</param>
        /// <param name="price">The price.</param>
        /// <param name="discount">The discount.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
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