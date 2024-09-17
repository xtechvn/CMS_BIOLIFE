using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ElasticSearch
{
    public class FlyBookingESViewModel
    {
        public long id { get; set; }
        public long orderid { get; set; }
        public string status { get; set; }
        public string bookingcode { get; set; }
        public string flight { get; set; }
        public double amount { get; set; }
        public DateTime expirydate { get; set; }
        public int leg { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string servicecode { get; set; }
        public string groupbookingid { get; set; }
        public string startpoint { get; set; }
        public string endpoint { get; set; }
    }
}
