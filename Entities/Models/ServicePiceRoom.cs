using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class ServicePiceRoom
    {
        public int Id { get; set; }
        public string RoomId { get; set; }
        public int? RoomPackageId { get; set; }
        public string HotelId { get; set; }
        public string RoomName { get; set; }
        public string RoomCode { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateLast { get; set; }

        public virtual RoomPackage RoomPackage { get; set; }
    }
}
