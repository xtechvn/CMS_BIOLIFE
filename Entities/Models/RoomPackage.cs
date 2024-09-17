using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class RoomPackage
    {
        public RoomPackage()
        {
            ServicePiceRoom = new HashSet<ServicePiceRoom>();
        }

        public int Id { get; set; }
        public string PackageId { get; set; }
        public string Code { get; set; }
        public int? RoomFunId { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateLast { get; set; }

        public virtual RoomFun RoomFun { get; set; }
        public virtual ICollection<ServicePiceRoom> ServicePiceRoom { get; set; }
    }
}
