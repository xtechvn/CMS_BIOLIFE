using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.SupplierConfig
{
    public class SupplierTicketSearchModel
    {
        public int supplier_id { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
    }

    public class SupplierTicketGridViewModel
    {

        public int Id { get; set; }
        public string PaymentCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public int TotalRow { get; set; }
    }
}
