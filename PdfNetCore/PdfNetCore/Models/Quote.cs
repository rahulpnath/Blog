using System;
using System.Collections.Generic;

namespace PdfNetCore.Models
{
    public class Quote
    {
        public string Number { get; set; }
        public DateTime CreatedDate { get; set; }
        public Customer Customer { get; set; } = new Customer();
        public List<QuoteItem> Items { get; set; } = new List<QuoteItem>();
        public string Notes { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class QuoteItem
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class Customer
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
