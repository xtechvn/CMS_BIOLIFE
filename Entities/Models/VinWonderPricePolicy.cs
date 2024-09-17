using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class VinWonderPricePolicy
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int ServiceId { get; set; }
        public double BasePrice { get; set; }
        public double WeekendRate { get; set; }
        public double Profit { get; set; }
        public short UnitType { get; set; }
        public int RateCode { get; set; }
        public string ServiceCode { get; set; }
        public string Name { get; set; }
        public double AmountBase { get; set; }
        public double AmountWeekend { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? SiteId { get; set; }
        public string SiteName { get; set; }

        public virtual Campaign Campaign { get; set; }
    }
}
