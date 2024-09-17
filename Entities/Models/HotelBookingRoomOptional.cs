using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class HotelBookingRoomOptional
    {
        public long Id { get; set; }
        public long HotelBookingId { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }
        public string RoomTypeName { get; set; }
        public string RatePlanId { get; set; }
        public string RatePlanCode { get; set; }
        public double Price { get; set; }
        public short? Nights { get; set; }
        public short? NumberOfRooms { get; set; }
        public double Profit { get; set; }
        public double TotalAmount { get; set; }
        public long? SupplierId { get; set; }
        public double? TotalDiscount { get; set; }
        public double? TotalOthersAmount { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
