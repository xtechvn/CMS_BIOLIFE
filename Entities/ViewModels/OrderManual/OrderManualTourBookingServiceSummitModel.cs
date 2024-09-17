using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{

    public class OrderManualTourBookingServiceSummitModel
    {
        public long order_id { get; set; }
        public long tour_id { get; set; }
        public int tour_type { get; set; }
        public int client_type { get; set; }
        public int organizing_type { get; set; }
        public int main_staff { get; set; }
        public int user_summit { get; set; }
        public int is_self_designed { get; set; }
        public int start_point { get; set; }
        public string end_point { get; set; }
        public string tour_product_name { get; set; }
        
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string service_code { get; set; }
        public long tour_product_id { get; set; }
        public string note { get; set; }

        public List<OrderManualTourBookingServiceSummitPassenger> guest { get; set; }
        public List<OrderManualTourBookingServiceSummitRouteExtraPackage> extra_packages { get; set; }

        public double other_amount { get; set; }
        public double commission { get; set; }

    }
    public class OrderManualTourBookingServiceSummitPassenger
    {
        public int id { get; set; }
        public string name { get; set; }
        public short gender { get; set; }
        public DateTime? birthday { get; set; }
        public string cccd { get; set; }
        public string room_number { get; set; }
        public string note { get; set; }
    }

 

    public class OrderManualTourBookingServiceSummitRouteExtraPackage
    {
        public long id { get; set; }
        public string package_id { get; set; }
        public string package_code { get; set; }
        public int quantity { get; set; }
        public double base_price { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public double profit { get; set; }
    }
}
