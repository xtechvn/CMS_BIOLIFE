using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class HotelBookingRoomExtraPackages
    {
        public long Id { get; set; }
        public string PackageId { get; set; }
        public string PackageCode { get; set; }
        public long? HotelBookingId { get; set; }
        public long? HotelBookingRoomId { get; set; }
        public double? Amount { get; set; }
        public double? UnitPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Profit { get; set; }
        public int? PackageCompanyId { get; set; }
        public double? OperatorPrice { get; set; }
        public double? SalePrice { get; set; }
        public short? Nights { get; set; }
        public int? Quantity { get; set; }
        public int? SupplierId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
