using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class HotelBookingRoomRates
    {
        public long Id { get; set; }
        public long HotelBookingRoomId { get; set; }
        public string RatePlanId { get; set; }
        public DateTime StayDate { get; set; }
        public double Price { get; set; }
        public double Profit { get; set; }
        public double TotalAmount { get; set; }
        public string AllotmentId { get; set; }
        public string RatePlanCode { get; set; }
        public string PackagesInclude { get; set; }
        public double? UnitPrice { get; set; }
        public short? Nights { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? OperatorPrice { get; set; }
        public double? SalePrice { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
