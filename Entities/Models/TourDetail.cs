using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class TourDetail
    {
        public long Id { get; set; }
        public long TourId { get; set; }
        public int? Days { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
