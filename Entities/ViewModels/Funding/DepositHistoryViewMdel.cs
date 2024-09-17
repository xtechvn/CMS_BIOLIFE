using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class DepositHistoryViewMdel : DepositHistory
    {
        public string paymentName { get; set; }
        public string statusName { get; set; }
    }
    
}
