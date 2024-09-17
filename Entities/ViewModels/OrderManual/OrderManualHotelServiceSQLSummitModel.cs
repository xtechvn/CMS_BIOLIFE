using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{
    public class OrderManualHotelServiceSQLSummitModel
    {
        public long order_id { get; set; }
        public Entities.Models.HotelBooking detail { get; set; }
        public ContactClient contact_client { get; set; }
        public List<OrderManualHotelServiceSQLSummitModelRoom> rooms { get; set; }
        public List<HotelBookingRoomExtraPackages> extra_packages { get; set; }
        public List<HotelGuest> booking_guests { get; set; }

    }

    public class OrderManualHotelServiceSQLSummitModelRoom
    {
        public  HotelBookingRooms detail { get; set; }
        public List<HotelBookingRoomRates> rates { get; set; }
        public List<HotelGuest> guests { get; set; }
        public List<HotelBookingRoomExtraPackages> extra_packages { get; set; }
    }
}
