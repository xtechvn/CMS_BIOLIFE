using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public enum FeeBuyType
    {       
        FIRST_POUND_FEE = 1,
        NEXT_POUND_FEE = 2,
        LUXURY_FEE = 3,
        DISCOUNT_FIRST_FEE = 4,
        TOTAL_SHIPPING_FEE = 5,
        SHIPPING_US_FEE = 6,
        PRICE_LAST= 7,
        ITEM_WEIGHT = 8 // lưu cân nặng đã quy đổi sang POUND
    }
}
