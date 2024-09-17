using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class RoomFun
    {
        public RoomFun()
        {
            RoomPackage = new HashSet<RoomPackage>();
        }

        public int Id { get; set; }
        public string AllotmentId { get; set; }
        public string AllotmentName { get; set; }
        public string HotelId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateLast { get; set; }
        public int? GroupProviderType { get; set; }
        public int? ContractType { get; set; }
        public string HotelName { get; set; }

        public virtual ICollection<RoomPackage> RoomPackage { get; set; }
    }
}
