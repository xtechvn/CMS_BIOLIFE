using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceRequestHistoryViewModel : InvoiceRequestHistory
    {
        public string UserCreateName { get; set; }
    }
}
