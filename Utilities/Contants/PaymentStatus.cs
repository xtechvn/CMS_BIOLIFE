using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Utilities.Contants
{
    public enum PaymentStatus
    {
        [Description("Chưa thanh toán")]
        UNPAID = 0,

        [Description("Đã thanh toán")]
        PAID = 1,

        [Description("Thanh toán thiếu")]
        PAID_NOT_ENOUGH = 2

    }
}
