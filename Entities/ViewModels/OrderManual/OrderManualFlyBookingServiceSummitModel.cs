using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{

    public class OrderManualFlyBookingServiceSummitModel
    {
        public long order_id { get; set; }
        public int client_type { get; set; }
        public long fly_booking_id { get; set; }
        public int route { get; set; }
        public int main_staff { get; set; }
        public int user_summit { get; set; }

        public string start_point { get; set; }
        public string end_point { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public OrderManualFlyBookingServiceSummitRoute go { get; set; }
        public OrderManualFlyBookingServiceSummitRoute back { get; set; }
        public List<OrderManualFlyBookingServiceSummitPassenger> passenger { get; set; }
        public List<OrderManualFlyBookingServiceSummitRouteExtraPackage> extra_packages { get; set; }
        public double amount_adt { get; set; }
        public double amount_chd { get; set; }
        public double amount_inf { get; set; }
        public double profit { get; set; }
        public string group_id { get; set; }
        public string service_code { get; set; }
        public string note { get; set; }
        public double? others_amount { get; set; }
        public double? commission { get; set; }

    }
    public class OrderManualFlyBookingServiceSummitPassenger
    {
        public int id { get; set; }
        public int genre { get; set; }
        public string name { get; set; }
        public DateTime? birthday { get; set; }
        public string note { get; set; }
    }

    public class OrderManualFlyBookingServiceSummitRoute
    {
        public long id { get; set; }
        public string airline { get; set; }
        public string fly_code { get; set; }
        public string booking_code { get; set; }
     
    }

    public class OrderManualFlyBookingServiceSummitRouteExtraPackage
    {
        public long id { get; set; }
        public string package_id { get; set; }
        public string package_code { get; set; }
        public double base_price { get; set; }
        public int quantity { get; set; }
        public double amount { get; set; }
        public double profit { get; set; }

    }
}
