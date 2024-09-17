using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
  public enum SortOrder
    {
       
        TRUY_CAP = 1,//Truy cập
        THEM = 2,//Thêm
        SUA = 3,//Sửa
        XOA = 4,//Xóa
        XUAT_BC = 5,//Xuất báo cáo
        VIEW_ALL = 6,//View All Data
        DUYET = 7,//Duyệt
    }
    public enum MenuId
    {
        HOP_DONG = 54,//Hợp đồng
        PHIEU_YEU_CAU_CHI = 64,//yêu cầu chi
        PHIEU_CHI = 62,//phiếu chi
        NAP_QUY = 46,//NẠP QUỸ
        QL_KHACH_HANG = 2,//Quản lý khách hàng
        PHIEU_THU = 57,//quản lý phiếu thu
        YEU_CAU_XUAT_HOA_DON = 77,//quản lý phiếu thu
        CHUONG_TRINH = 90,//quản chương trình qc
        //CHUONG_TRINH = 82,//quản chương trình pro

    }
}
