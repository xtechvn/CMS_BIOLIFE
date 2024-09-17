using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class FlyBookingExtraPackages
    {
        public long Id { get; set; }
        public string PackageId { get; set; }
        public string PackageCode { get; set; }
        public string GroupFlyBookingId { get; set; }
        public decimal Amount { get; set; }
        public decimal? BasePrice { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? Profit { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
