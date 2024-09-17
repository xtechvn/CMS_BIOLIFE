using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceDetailViewModel : InvoiceDetail
    {
        public long InvoiceDetailId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? VATAmount { get; set; }
        public double? TotalPriceVAT { get; set; }
        public double? TotalPrice { get; set; }
        public decimal? PriceExtra { get; set; }
        public decimal? PriceExtraExport { get; set; }
        public int? Vat { get; set; }
    }
}
