using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class AirPortCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DistrictEn { get; set; }
        public string DistrictVi { get; set; }
        public int? CountryId { get; set; }
    }
}
