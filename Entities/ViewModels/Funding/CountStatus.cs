using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class CountStatus
    {
        public int Status { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public long DataId { get; set; }
        public string DataIdFly { get; set; }
        public string DataNo { get; set; }
        public string StatusName { get; set; }
        public int ServiceType { get; set; }
        public string PaymentRequestStatus { get; set; }
    }
}
