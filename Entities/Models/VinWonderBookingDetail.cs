using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class VinWonderBookingDetail
    {
        public long Id { get; set; }
        public long? BookingId { get; set; }
        public string Code { get; set; }
        public string ServiceKey { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string TypeName { get; set; }
        public double? BasePrice { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
        public double? RateDiscountPercent { get; set; }
        public double? RateDiscountPrice { get; set; }
        public double? PromotionDiscountPercent { get; set; }
        public double? PromotionDiscountPrice { get; set; }
        public double? Vatpercent { get; set; }
        public int? Availability { get; set; }
        public int? NumberOfUses { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string WeekDays { get; set; }
        public string GateCode { get; set; }
        public string GateName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
