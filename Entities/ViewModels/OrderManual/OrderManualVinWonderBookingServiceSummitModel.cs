using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{

    public class OrderManualVinWonderBookingServiceSummitSQLModel
    {
        public Entities.Models.VinWonderBooking detail { get; set; }
        public List<VinWonderBookingTicket> packages { get; set; }
        public List<VinWonderBookingTicketCustomer> guests { get; set; }
    }

  public class OrderManualVinWonderBookingServiceSummitModel
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public string service_code { get; set; }
        public long location_id { get; set; }
        public string location_name { get; set; }
        public string note { get; set; }
        public int operator_id { get; set; }
        public List<OrderManualVinWonderBookingServiceSummitPackage> packages { get; set; }
        public List<OrderManualVinWonderBookingServiceSummitPassenger> guest { get; set; }
        public double others_amount { get; set; }
        public double commission { get; set; }

    }
    public class OrderManualVinWonderBookingServiceSummitPassenger
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string note { get; set; }
    }


    public class OrderManualVinWonderBookingServiceSummitPackage
    {
        public long id { get; set; }
        public string package_name { get; set; }
        public double base_price { get; set; }
        public int quantity { get; set; }
        public double amount { get; set; }
        public double profit { get; set; }
        public DateTime date_used { get; set; }

    }
}
