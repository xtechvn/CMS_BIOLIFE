using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.SupplierConfig
{
    public class SupplierOrderSearchModel
    {
        public int supplier_id { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
    }

    public class SupplierServiceSearchModel
    {
        public int supplier_id { get; set; }
        public string service_name { get; set; }
        public int service_type { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
    }

    public class SupplierOrderGridViewModel
    {
        public long OrderId { get; set; }
        public string ServiceCode { get; set; }
        public long ServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrderNo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountPay { get; set; }
        public string ServiceName { get; set; }
        public int? ServiceType { get; set; }
        public string SalerName { get; set; }
        public string GroupBookingId { get; set; }
        public int TotalRow { get; set; }
    }
}
