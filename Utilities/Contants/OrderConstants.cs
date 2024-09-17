using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public class OrderConstants
    {
        public enum ORDER_STATUS
        {
            DON_HANG_KHOI_TAO = 0,
            DON_HANG_CHO_THANH_TOAN = 1, //Đơn hàng chờ thanh toán
            DON_HANG_DA_THANH_TOAN = 2, //Đơn hàng đã thanh toán
            DON_HANG_THANH_CONG = 3, //Đơn hàng thành công
            DON_HANG_HET_HAN = 4, //Đơn hàng hết hạn
            DON_HANG_DA_HUY = 5,
            CHO_DUYET_CONG_NO = 6, //Chờ duyệt công nợ
            CONG_NO_THANH_TOAN_DU = 7,//Công nợ - thanh toán đủ
            CONG_NO_THANH_TOAN_THIEU = 8,//Công nợ - thanh toán thiếu
            KHONG_CONG_NO_THANH_TOAN_DU = 9, //Không công nợ - thanh toán đủ
            KHONG_CONG_NO_THANH_TOAN_THIEU = 10, //Không công nợ - thanh toán thiếu
            TRA_CODE_CONG_NO_THANH_TOAN_DU = 11, //Trả code công nợ - thanh toán đủ
            TRA_CODE_CONG_NO_THANH_TOAN_THIEU = 12, //Trả code công nợ - thanh toán thiếu
            TRA_CODE_KHONG_CONG_NO = 13, //Trả code không công nợ
            CHO_KIEM_DUYET_B2C = 14, //Chờ kiểm duyệt đơn hàng B2C
            DA_KIEM_DUYET_B2C = 15, //Đã kiểm duyệt đơn hàng B2C
            CHO_HOAN_THANH = 17, //Chờ hoàn thành
        }

    }
}
