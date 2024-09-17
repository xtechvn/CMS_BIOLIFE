using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.OrderDetail
{
    public class ListOrderDetailViewModel
    {
        public long OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public long Amount { get; set; }
        public long Price { get; set; }
        public long Profit { get; set; }
        public long Quantity { get; set; }
        public long TotalAmount { get; set; }
        public long TotalPrice { get; set; }
        public long TotalProfit { get; set; }
        public string Link { get; set; }
    }
}
