using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public static class CampaignStatus
    {
        public static Dictionary<string, string> CampaignStatusConstant { get; } = new Dictionary<string, string>
        {
             { "0", "Chờ chạy"},
             {"1", "Kết thúc"},
             {"2", "Đang chạy"},
             { "3", "Khóa / Tạm dừng"},
        };
    }
}
