using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class OtherBookingPackages
    {
        public long Id { get; set; }
        public long BookingId { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public double? Profit { get; set; }
        public double? SalePrice { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ServiceType { get; set; }
        public string Note { get; set; }
        public decimal? Commission { get; set; }
    }
}
