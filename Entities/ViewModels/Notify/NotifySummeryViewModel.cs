using System;
using System.Collections.Generic;
using System.Text;

namespace ENTITIES.ViewModels.Notify
{
   public class NotifySummeryViewModel
    {
        public int total_not_seen { get; set; } // Tổng số noti chưa đọc
        public string lst_id_not_seen { get; set; } //  list các id noti chưa đọc
        public List<ReceiverMessageViewModel> lst_not_seen_detail { get; set; } // Danh sách chi tiết các notify chưa đọc và đã đọc view all
    }
    public class NotifyRedisViewModel
    {
        public int status { get; set; }
     
        public NotifySummeryViewModel data { get; set; } 
    }
}
