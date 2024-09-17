using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{
    public class OrderManualSummitViewModel
    {
        public long client_id { get; set; }
        public long main_sale_id { get; set; }
        public List<long> sub_sale_id { get; set; }
        public short branch { get; set; }

        public string order_source { get; set; } = "CMS";
        public string note { get; set; }
        public string label { get; set; }
    }
}
