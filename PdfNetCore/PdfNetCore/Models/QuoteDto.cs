using System;
using System.Collections.Generic;

namespace PdfNetCore.Models
{
    public class QuoteDto
    {
        public string Number { get; set; }
        public DateTime CreatedDate { get; set; }
        public CustomerDto Customer { get; set; } = new CustomerDto();
        public List<QuoteItemDto> Items { get; set; } = new List<QuoteItemDto>();
        public string Notes { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class QuoteItemDto
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CustomerDto
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
