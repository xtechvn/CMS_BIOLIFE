using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ElasticSearch
{
    public class ESHotelBookingCodeViewModel
    {
        public long id { get; set; } // ID ElasticSearch
        public long serviceid { get; set; }
        public long hotel_type { get; set; }
        public string bookingcode { get; set; }
        public string description { get; set; }
        public bool IsDelete { get; set; }
    }
}
