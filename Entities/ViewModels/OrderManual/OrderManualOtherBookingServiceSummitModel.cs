using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{

    public class OrderManualOtherBookingServiceSummitSQLModel
    {
        public Entities.Models.OtherBooking booking { get; set; }
        public List<OtherBookingPackages> packages { get; set; }
    }

        public class OrderManualOtherBookingServiceSummitModel
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public string service_code { get; set; }
        public int service_type { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public string note { get; set; }
        public int operator_id { get; set; }
        public List<OrderManualOtherBookingServiceSummitPackage> packages { get; set; }
        public double others_amount { get; set; }
        public double commission { get; set; }
    }
   

    public class OrderManualOtherBookingServiceSummitPackage
    {
        public long id { get; set; }
        public string package_name { get; set; }
        public double base_price { get; set; }
        public int quantity { get; set; }
        public double amount { get; set; }
        public double profit { get; set; }
        public double sale_price { get; set; }
    }
}
