using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public enum PaymentType
    {
        CHUYEN_KHOAN_TRUC_TIEP = 1,//: Chuyển khoản ngân hàng
        ATM = 2,// Thẻ ATM/Tài khoản ngân hàng
        VISA_MASTER_CARD = 3,// Thẻ VISA/Master Card
        QR_PAY = 4,// Thanh toán QR/PAY
        KY_QUY = 5,// Thanh toán bằng ký quỹ
        GIU_CHO = 6,// Giữ chỗ
        TAI_VAN_PHONG = 7//Thanh toán tại văn phòng

    }

}
