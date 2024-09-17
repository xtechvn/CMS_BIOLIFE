using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class FlightSegment
    {
        public long Id { get; set; }
        public long FlyBookingId { get; set; }
        public string OperatingAirline { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string FlightNumber { get; set; }
        public int Duration { get; set; }
        public string Class { get; set; }
        public string Plane { get; set; }
        public string StartTerminal { get; set; }
        public string EndTerminal { get; set; }
        public string StopPoint { get; set; }
        public string HandBaggage { get; set; }
        public string AllowanceBaggage { get; set; }
        public double? StopTime { get; set; }
        public bool? HasStop { get; set; }
        public bool? ChangeStation { get; set; }
        public bool? ChangeAirport { get; set; }
        public bool? StopOvernight { get; set; }
        public double? AllowanceBaggageValue { get; set; }
        public double? HandBaggageValue { get; set; }
    }
}
