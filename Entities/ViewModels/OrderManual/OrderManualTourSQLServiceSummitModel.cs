using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{

    public class OrderManualTourSQLServiceSummitModel
    {
        public Entities.Models.Tour detail { get; set; }
        public List<TourPackages> extra_packages { get; set; }
        public List<TourGuests> guest { get; set; }
        public TourProduct product { get; set; }
        public List<TourDestination> destinations { get; set; }
    }
   
}
