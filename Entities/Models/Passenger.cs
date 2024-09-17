using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MembershipCard { get; set; }
        public string PersonType { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Gender { get; set; }
        public long OrderId { get; set; }
        public string Note { get; set; }
        public string GroupBookingId { get; set; }

        public virtual Order Order { get; set; }
    }
}
