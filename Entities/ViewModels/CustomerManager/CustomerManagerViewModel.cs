using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.CustomerManager
{
    public class CustomerManagerViewModel : Entities.Models.Client
    {
        public double sum_payment { get; set; }
        public string client_type_name { get; set; }
        public string UserId_name { get; set; }
        public string Create_name { get; set; }
        public string create_payment { get; set; }
        public string AgencyType_name { get; set; }
        public string PermisionType_name { get; set; }
        public string CreateDate_UserAgent { get; set; }
        public string UpdateLast { get; set; }
        public string Update_Name { get; set; }
        public string VerifyDate { get; set; }
        public string VerifyStatus_name { get; set; }
        public long UserId { get; set; }
        public long ACStatus { get; set; }
     
        public long TotalAmount { get; set; }
       
    }
    public class CustomerManagerViewSearchModel
    {
        public int MaKH { get; set; } = -1;
        public int CreatedBy { get; set; } = -1;
        public int UserId { get; set; } = -1;
        public string TenKH { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AgencyType { get; set; } = -1;
        public int ClientType { get; set; } = -1;
        public int PermissionType { get; set; } = -1;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string CreateDate { get; set; }
        public string EndDate { get; set; }
        public double MinAmount { get; set; } = -1;
        public double MaxAmount { get; set; } = -1;
        public string SalerPermission { get; set; }
        public string CacheName { get; set; }
        public string _id { get; set; }
    }

    public class CustomerManagerView
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int AgencyType { get; set; }
        public string Client_name { get; set; }
        public string Maso_Id { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string diachi_chinhanh { get; set; }
        public string DC_hoadon { get; set; }
        public string DiaChi_giaodich { get; set; }
        public int PermisionType { get; set; }
        public string id_loaikhach { get; set; }
        public string id_nhomkhach { get; set; }
        public string id_ClientType { get; set; }
        public string pass { get; set; }
        public string pass_backup { get; set; }
        public string So_tk { get; set; }
        public string Name_tk { get; set; }
        public string Name_nh { get; set; }
        public string Note { get; set; }
        public string ClientCode { get; set; }
        public DateTime JoinDate { get; set; }

    }
    public class AmountRemainView
    {
        public double AmountRemain { get; set; }

    }
    public class field
    {
        public bool STT { get; set; }
        public bool MaKH { get; set; }
        public bool TenKH { get; set; }
        public bool LienHe { get; set; }
        public bool DoiTuong { get; set; }
        public bool LoaiKH { get; set; }
        public bool NhomKH { get; set; }
        public bool TongTT { get; set; }
        public bool VNPhuTrach { get; set; }
        public bool NgayTao { get; set; }
        public bool NgayCN { get; set; }
        public bool NgayDuyet { get; set; }
        public bool NguoiTao { get; set; }
        public bool Status { get; set; }

    }
}
