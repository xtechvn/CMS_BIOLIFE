using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public struct ProductType
    {   
        public const int AUTO = 0; // job tự động crawl sp
        public const int MANUAL_EDIT = 1; // tạo sp thủ công
        public const int MANUAL_AUTO = 2; // map báo giá thủ công
        public const int CRAWL_EXTENSION = 3; // crawl từ extension chrome
        public const int FIXED_AMOUNT_VND = 4; // các sản phẩm có giá về tay (VNĐ) cố định, không đổi.
    }
}
