using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class PriceDetail
    {
        public int Id { get; set; }
        public int ProductServiceId { get; set; }
        public double Price { get; set; }
        public double Profit { get; set; }
        public short? UnitId { get; set; }
        public double? AmountLast { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? UserCreateId { get; set; }
        public int? UserUpdateId { get; set; }
        public string MonthList { get; set; }
        public string DayList { get; set; }
        public short ServiceType { get; set; }
    }
}
