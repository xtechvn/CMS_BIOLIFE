using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class HotelBookingCode
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public long? ServiceId { get; set; }
        public int? Type { get; set; }
        public string BookingCode { get; set; }
        public string Description { get; set; }
        public string AttactFile { get; set; }
        public bool? IsDelete { get; set; }
        public string Note { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
