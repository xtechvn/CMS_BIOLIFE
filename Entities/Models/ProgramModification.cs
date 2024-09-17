using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class ProgramModification
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long ProgramId { get; set; }
        public int SupplierId { get; set; }
        public int ServiceType { get; set; }
        public int? Status { get; set; }
        public int? UserVerify { get; set; }
        public DateTime? VerifyDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
