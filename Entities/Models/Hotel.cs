using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class Hotel
    {
        public int Id { get; set; }
        public string HotelId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageThumb { get; set; }
        public short? NumberOfRoooms { get; set; }
        public decimal? Star { get; set; }
        public int? ReviewCount { get; set; }
        public decimal? ReviewRate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string HotelType { get; set; }
        public string TypeOfRoom { get; set; }
        public bool? IsRefundable { get; set; }
        public bool? IsInstantlyConfirmed { get; set; }
        public string GroupName { get; set; }
        public string Telephone { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public int? SupplierId { get; set; }
        public int? ProvinceId { get; set; }
        public string TaxCode { get; set; }
        public int? EstablishedYear { get; set; }
        public int? RatingStar { get; set; }
        public int? ChainBrands { get; set; }
        public int? VerifyDate { get; set; }
        public int? SalerId { get; set; }
        public bool? IsDisplayWebsite { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ListSupplierId { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Extends { get; set; }
        public bool? IsVinHotel { get; set; }
        public string OtherSurcharge { get; set; }
        public bool? IsCommitFund { get; set; }
    }
}
