using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{
   
    public class OrderManualHotelSerivceSummitHotel
    {
        public long order_id { get; set; }
        public OrderManualHotelSerivceSummitHotelDetail hotel { get; set; }
        
        public List<OrderManualHotelSerivceSummitHotelRoom> rooms { get; set; }
        public List<OrderManualHotelSerivceSummitExtraPackages> extra_package { get; set; }
       // public OrderManualHotelSerivceSummitContactClient contact_client { get; set; }
        public List<OrderManualHotelSerivceSummitGuest> guest { get; set; }
    }
    public class OrderManualHotelSerivceSummitHotelDetail
    {
        public long id { get; set; }
        public string hotel_id { get; set; }
        public string service_code { get; set; }
        public string hotel_name { get; set; }
        public int number_of_rooms { get; set; }
        public DateTime arrive_date { get; set; }
        public DateTime departure_date { get; set; }
        public long main_staff_id { get; set; }
        public int number_of_adult { get; set; }
        public int number_of_child { get; set; }
        public int number_of_infant { get; set; }
        public string note { get; set; }
        public double other_amount { get; set; }
        public double discount { get; set; }
        
    }
    public class OrderManualHotelSerivceSummitHotelRoom
    {
        public long id { get; set; }
        public int room_no { get; set; }
        public string room_type_id { get; set; }
        public string room_type_code { get; set; }
        public string room_type_name { get; set; }
        public short number_of_rooms { get; set; }
        public List<OrderManualHotelSerivceSummitHotelRoomRate> package { get; set; }

    }
    public class OrderManualHotelSerivceSummitHotelRoomRate
    {
        public long id { get; set; }
        public string package_code { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public double operator_price { get; set; }
        public double sale_price { get; set; }
        public double amount { get; set; }
        public double profit { get; set; }
        public short nights { get; set; }

    }
    public class OrderManualHotelSerivceSummitExtraPackages
    {
        public long id { get; set; }
        public int package_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public double operator_price { get; set; }
        public double sale_price { get; set; }
        public int nights { get; set; }
        public int number_of_extrapackages { get; set; }
        public double amount { get; set; }
        public double profit { get; set; }
        
    }
    public class OrderManualHotelSerivceSummitContactClient
    {
        public long id { get; set; }
        public long client_id { get; set; }
        public long order_id { get; set; }
        public string name { get; set; }
        public DateTime birthday { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

    } 
    public class OrderManualHotelSerivceSummitGuest
    {
        public long id { get; set; }

        public string name { get; set; }
        public DateTime? birthday { get; set; }
        public int room_no { get; set; }
        public string note { get; set; }
        public short type { get; set; }


    }

}
