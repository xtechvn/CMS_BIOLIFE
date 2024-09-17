using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class PlaygroundDetail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ServiceType { get; set; }
        public int NewsId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string LocationName { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
    }
}
