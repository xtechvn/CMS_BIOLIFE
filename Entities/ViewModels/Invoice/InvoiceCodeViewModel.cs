using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceCodeViewModel
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceCode { get; set; }
        public long OrderId { get; set; }
    }
}
