using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class VinWonderBookingTicket
    {
        public long Id { get; set; }
        public long? BookingId { get; set; }
        public string RateCode { get; set; }
        public int? Number { get; set; }
        public double? Amount { get; set; }
        public double? Profit { get; set; }
        public DateTime? DateUsed { get; set; }
        public int? Adt { get; set; }
        public int? Child { get; set; }
        public int? Old { get; set; }
        public double? TotalPrice { get; set; }
        public double? UnitPrice { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; }
        public double? BasePrice { get; set; }
        public int? Quantity { get; set; }
    }
}
