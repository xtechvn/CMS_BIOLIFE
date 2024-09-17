using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class DepositHistoryConstant
    {
        public enum DEPOSIT_STATUS
        {
            TAO_MOI = 0,
            CHO_THANH_TOAN = 1,
            CHO_DUYET = 2,
            DA_DUYET = 3,
            TU_CHOI = 4,
            HET_HAN = 5,
        }
        public enum ORDER_STATUS
        {
            KHOI_TAO = 0,
            CHO_THANH_TOAN = 1, //Đơn hàng chờ thanh toán
            DA_THANH_TOAN = 2, //Đơn hàng đã thanh toán
            THANH_CONG = 3, //Đơn hàng thành công
            HET_HAN = 4, //Đơn hàng hết hạn
            DA_HUY = 5,
            CHO_DUYET = 6, //Chờ duyệt công nợ
            CN_TTD = 7,//Công nợ - thanh toán đủ
            CN_TTT = 8,//Công nợ - thanh toán thiếu
            KCN_TTD = 9, //Không công nợ - thanh toán đủ
            KCN_TTT = 10, //Không công nợ - thanh toán thiếu
            TRA_CODE_D = 11, //Trả code công nợ - thanh toán đủ
            TRA_CODE_T = 12, //Trả code công nợ - thanh toán thiếu
            KHONG_NO = 13, //Trả code không công nợ
            CHO_KIEM_DUYET = 14, //Chờ kiểm duyệt đơn hàng B2C
            DA_KIEM_DUYET = 15, //Đã kiểm duyệt đơn hàng B2C
        }


        public enum DEPOSIT_PAYMENT_TYPE
        {
            THANH_TOAN_DON_HANG = 0,
            NAP_QUY = 1,
        }


        public enum CONTRACT_PAY_STATUS
        {
            BOT_DUYET = 0,
            KE_TOAN_DUYET = 1,
            HUY = 2
        }

        public enum CONTRACT_PAY_TYPE
        {
            THU_TIEN_DON_HANG = 1,
            THU_TIEN_KY_QUY = 2,
            THU_TIEN_NCC_HOAN_TRA = 3,
            THU_TIEN_HOA_HONG_NCC = 4,
            THU_KHAC = 5,
        }

        public enum CONTRACT_PAY_OBJECT_TYPE
        {
            KHACH_HANG = 1,
            NHA_CUNG_CAP = 2,
            NHAN_VIEN = 3,
        }


        public enum CONTRACT_PAYMENT_TYPE
        {
            TIEN_MAT = 1,
            CHUYEN_KHOAN = 2,
        }

        public enum ORDER_DEBT_STATUS
        {
            GACH_NO_DU = 1,
            GACH_NO_CHUA_DU = 2,
        }

        public enum CONTRACTPAY_DEBT_STATUS
        {
            DA_GACH_HET = 1,
            CHUA_GACH_HET = 2,
        }
    }
}
