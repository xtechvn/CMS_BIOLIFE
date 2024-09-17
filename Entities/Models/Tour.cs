using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class Tour
    {
        public long Id { get; set; }
        public int? TourType { get; set; }
        public int? OrganizingType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? OrderId { get; set; }
        public int? SalerId { get; set; }
        public double? Amount { get; set; }
        public string ServiceCode { get; set; }
        public int? SupplierId { get; set; }
        public int? Status { get; set; }
        public int? Days { get; set; }
        public double? Price { get; set; }
        public int? Star { get; set; }
        public string Avatar { get; set; }
        public bool? IsDisplayWeb { get; set; }
        public string Image { get; set; }
        public string Schedule { get; set; }
        public string AdditionInfo { get; set; }
        public long? TourProductId { get; set; }
        public double? Profit { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Note { get; set; }
        public int? StatusOld { get; set; }
        public double? Commission { get; set; }
        public double? OthersAmount { get; set; }
    }
}
