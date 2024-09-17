using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class SupplierViewModel : Supplier
    {
        public string ServiceTypeName { get; set; }
        public string ServiceName { get; set; }
        public string ResidenceName { get; set; }
        public string ProvinceName { get; set; }
        public string SalerName { get; set; }
        public string CreatedName { get; set; }
        public string LocalVerifyDate { get; set; }
        public string UpdatedName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string VerifyDateName { get; set; }
        public long TotalRow { get; set; }
        public int id { get; set; }
        public string name
        {
            get
            {
                var nameStr = fullname;
                if (!string.IsNullOrEmpty(Email))
                    nameStr += " - " + Email;
                if (!string.IsNullOrEmpty(Phone))
                    nameStr += " - " + Phone;
                return nameStr;
            }
        }
        public string fullname { get; set; }
    }

    public class SupplierDetailViewModel : Supplier
    {
        public string SalerName { get; set; }
        public string SalerMail { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedByMail { get; set; }

        public string UpdatedByName { get; set; }
        public string UpdatedByMail { get; set; }

        public string ServiceName { get; set; }
        public string ResidenceName { get; set; }

        public string BrandName { get; set; }
        public string VerifyDateName { get; set; }
    }

    public class SupplierRoomModel
    {
        public long Id { get; set; }
        public long HotelId { get; set; }
        public string RoomId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Thumbnails { get; set; }
        public string RoomAvatar { get; set; }
        public string Code { get; set; }
        public int? NumberOfBedRoom { get; set; }
        public string Description { get; set; }
        public string TypeOfRoom { get; set; }
        public string Extends { get; set; }
        public int? BedRoomType { get; set; }
        public int? NumberOfAdult { get; set; }
        public int? NumberOfChild { get; set; }
        public int? NumberOfRoom { get; set; }
        public decimal? RoomArea { get; set; }
        public bool IsActive { get; set; }
        public bool IsDisplayWebsite { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class SupplierRoomViewModel : SupplierRoomModel
    {
        public IEnumerable<string> OtherImages { get; set; }
    }

    public class SupplierRoomGridModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TypeOfRoom { get; set; }
        public int NumberOfAdult { get; set; }
        public int NumberOfChild { get; set; }
        public int NumberOfRoom { get; set; }
        public string Description { get; set; }
        public int BedRoomType { get; set; }
        public string BedRoomTypeName { get; set; }
        public decimal? RoomArea { get; set; }
        public int IsActive { get; set; }
        public int TotalRow { get; set; }
    }

    public class SupplierHotelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
