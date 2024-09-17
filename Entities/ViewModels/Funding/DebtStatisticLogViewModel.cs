using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class DebtStatisticLogViewModel
    {
        public long DebtStatisticId { get; set; }
        public string DebtStatisticCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
