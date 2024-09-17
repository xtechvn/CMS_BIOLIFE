using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class ProductRoomService
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public long? ProgramId { get; set; }
        public int? ProgramPackageId { get; set; }
        public int? HotelId { get; set; }
        public string AllotmentsId { get; set; }
        public string PackageCode { get; set; }
        public int? RoomId { get; set; }
    }
}
