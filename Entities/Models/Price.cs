using System;
using System.Collections.Generic;





namespace Entities.Models
{
    public partial class Price
    {
        public int PriceId { get; set; }
        public double Price1 { get; set; }
        public int UnitId { get; set; }
        public byte ServiceType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateLast { get; set; }
        public long UserUpdateId { get; set; }
        public int UserCreateId { get; set; }
    }
}
