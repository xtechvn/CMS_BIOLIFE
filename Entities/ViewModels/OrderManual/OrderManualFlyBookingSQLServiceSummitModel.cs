using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{

    public class OrderManualFlyBookingSQLServiceSummitModel
    {
        public long order_id { get; set; }
        public double total_amount { get; set; }
        public double profit { get; set; }
        public FlyBookingDetail go { get; set; }
        public FlyBookingDetail back { get; set; }
        public List<FlyBookingExtraPackages> extra_packages { get; set; }
        public List<Passenger> passengers { get; set; }
    }
   
}
