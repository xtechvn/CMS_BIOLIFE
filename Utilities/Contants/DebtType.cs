using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Utilities
{
    public enum DebtType
    {
        /// <summary>
        /// Trạng thái công nợ đối với Khách hàng B2B và tổng tiền đơn vẫn nằm trong mức công nợ
        /// </summary>
        [Description("Được công nợ")]
        DEBT_ACCEPTED = 1,
        /// <summary>
        /// Trạng thái công nợ đối với Khách hàng B2C / Khách hàng B2B nhưng tổng tiền đơn đã vượt quá công nợ	
        /// </summary>
        [Description("Không được công nợ")]
        DEBT_NOT_ACCEPTED = 0,

    }
}
