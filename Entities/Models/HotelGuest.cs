using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class HotelGuest
    {
        public long Id { get; set; }
        public long HotelBookingRoomsId { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public long HotelBookingId { get; set; }
        public string Note { get; set; }
        public short? Type { get; set; }
    }
}
