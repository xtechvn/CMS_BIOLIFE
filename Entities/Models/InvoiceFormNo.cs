using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class InvoiceFormNo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public bool? IsUsed { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
