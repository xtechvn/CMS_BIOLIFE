using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Payment
{
    public class PayooBankViewModel
    {
        public string account_number { get; set; } // so tai khoan
        public string branch { get; set; } // chi nhánh
        public string code { get; set; } // mã code ngân hàng visdu : tpBank
        public string name { get; set; } // tên ngân hàng: ví dụ:  VietCombank
        public string url_icon { get; set; } // icon ngan hang
    }
}
