using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class ProgramPackage
    {
        public int Id { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public int? ProgramId { get; set; }
        public string RoomType { get; set; }
        public int? RoomTypeId { get; set; }
        public double? Amount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? WeekDay { get; set; }
        public DateTime? ApplyDate { get; set; }
        public int? OpenStatus { get; set; }
        public double? Price { get; set; }
        public double? Profit { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
