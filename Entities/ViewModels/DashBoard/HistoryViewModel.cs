using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.DashBoard
{
    public class HistoryViewModel
    {
        public string UserVerify { get; set; }
        public string OrderNo { get; set; }
        public long OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? UpdateLast { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
