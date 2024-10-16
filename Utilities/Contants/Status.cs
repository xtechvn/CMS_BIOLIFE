using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public enum StatusType
    {
        BINH_THUONG = 0,
        KHOA_TAM_DUNG = 1
    }
    public enum ResponseType
    {
        SUCCESS = 0,
        FAILED = 1,
        ERROR = 2,
        EXISTS = 3,
        PROCESSING = 4,
        EMPTY = 5,
        CONFIRM = 6,
        NOT_EXISTS = 7
    }
    public enum Status
    {
        HOAT_DONG = 0,
        KHONG_HOAT_DONG = 1
    }

    public struct ArticleStatus
    {
        public const int PUBLISH = 0; // BÀI XUẤT BẢN
        public const int SAVE = 1; // BÀI LƯU TẠM
        public const int REMOVE = 2; // BÀI LƯU TẠM
    }

    public struct GroupProductStatus
    {
        public const int PUBLISH = 0; // BÀI XUẤT BẢN
        public const int REMOVE = 1; // BÀI LƯU TẠM
    }

    public enum PAYMENT_REQUEST_STATUS
    {
        LUU_NHAP = 0,
        TU_CHOI = 1,
        CHO_TBP_DUYET = 2,
        CHO_KTT_DUYET = 3,
        CHO_CHI = 4, //cho tao phieu chi
        DA_CHI = 5, //da tao phieu chi
    }

    public enum DEBT_STATISTIC_STATUS
    {
        LUU_NHAP = 0,
        TU_CHOI = 1,
        CHO_KE_TOAN_XAC_NHAN = 2,
        CHO_KHACH_HANG_XAC_NHAN = 3,
        HUY = 5,
        DA_XAC_NHAN = 4
    }

    public enum PAYMENT_VOUCHER_TYPE
    {
        THANH_TOAN_DICH_VU = 1,
        THANH_TOAN_KHAC = 2,
        HOAN_TRA_KHACH_HANG = 3,
        CHI_PHI_MARKETING = 4,
    }

    public enum INVOICE_REQUEST_STATUS
    {
        LUU_NHAP = 0,
        TU_CHOI = 1,
        CHO_TBP_DUYET = 2,
        DA_DUYET = 4,
        HOAN_THANH = 5,
        CHO_KTT_DUYET = 6,
    }

    public enum GET_CODE_MODULE
    {
        YEU_CAU_CHI = 5,
        PHIEU_CHI = 4,
        BANG_KE = 83,
        PHIEU_THU = 1,
        HOA_DON = 7,
        YEU_CAU_XUAT_HOA_DON = 6,
    }
}
