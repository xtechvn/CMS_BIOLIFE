using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class HotelBooking
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string BookingId { get; set; }
        public short? Status { get; set; }
        public string PropertyId { get; set; }
        public short? HotelType { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int NumberOfRoom { get; set; }
        public int NumberOfAdult { get; set; }
        public int NumberOfChild { get; set; }
        public int NumberOfInfant { get; set; }
        public double TotalPrice { get; set; }
        public double TotalProfit { get; set; }
        public double TotalAmount { get; set; }
        public string HotelName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImageThumb { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public double? ExtraPackageAmount { get; set; }
        public long? SalerId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ServiceCode { get; set; }
        public int? NumberOfPeople { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double? Price { get; set; }
        public int? SupplierId { get; set; }
        public string ReservationCode { get; set; }
        public string Note { get; set; }
        public int? StatusOld { get; set; }
        public double? TotalDiscount { get; set; }
        public double? TotalOthersAmount { get; set; }
    }
}
