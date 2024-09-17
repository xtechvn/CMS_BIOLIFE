using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{
    public class OrderManualTourProductViewModel: TourProduct
    {
        public List<OrderManualTourProductEndpoint> end_point_name { get; set; }
        public string start_point_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

    }

    public class OrderManualTourProductEndpoint
    {
        public long id { get; set; }
        public string endpoint_name { get; set; }
    }
}
