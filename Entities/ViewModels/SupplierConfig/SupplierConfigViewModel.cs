using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.SupplierConfig
{
    public class SupplierConfigViewModel
    {
    }

    public class SupplierConfigUpsertModel : Supplier
    {
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPosition { get; set; }

        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankId { get; set; }
        public string BankBranch { get; set; }
    }
}
