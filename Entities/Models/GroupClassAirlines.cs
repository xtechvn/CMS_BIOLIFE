using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class GroupClassAirlines
    {
        public int Id { get; set; }
        public string Airline { get; set; }
        public string ClassCode { get; set; }
        public string FareType { get; set; }
        public string DetailVi { get; set; }
        public string DetailEn { get; set; }
        public string Description { get; set; }
    }
}
