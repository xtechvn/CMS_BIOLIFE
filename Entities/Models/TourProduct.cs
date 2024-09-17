using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class TourProduct
    {
        public long Id { get; set; }
        public string TourName { get; set; }
        public string ServiceCode { get; set; }
        public int? TourType { get; set; }
        public int? StartPoint { get; set; }
        public int? OrganizingType { get; set; }
        public string DateDeparture { get; set; }
        public double Price { get; set; }
        public int? SupplierId { get; set; }
        public int? Status { get; set; }
        public int? Days { get; set; }
        public double? OldPrice { get; set; }
        public int? Star { get; set; }
        public string Avatar { get; set; }
        public bool? IsDisplayWeb { get; set; }
        public string Image { get; set; }
        public string Schedule { get; set; }
        public string AdditionInfo { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsSelfDesigned { get; set; }
        public string Transportation { get; set; }
        public string Description { get; set; }
        public string Include { get; set; }
        public string Exclude { get; set; }
        public string Refund { get; set; }
        public string Surcharge { get; set; }
        public string Note { get; set; }
    }
}
