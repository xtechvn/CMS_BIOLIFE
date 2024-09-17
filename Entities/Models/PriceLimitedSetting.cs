using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class PriceLimitedSetting
    {
        public int Id { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int? UnitId { get; set; }
        public int? ServiceType { get; set; }
    }
}
