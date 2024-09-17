using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class ContractPayDebtViewModel
    {
        public double Payment { get; set; }
        public long TotalRow { get; set; }
        public long PayId { get; set; }
        public string BIllNo { get; set; }
        public string ContractPayType { get; set; }
        public string PayTypeStr { get; set; }
        public string ClientName { get; set; }
        public double Amount { get; set; }
        public string PayDetail { get; set; }
        public string PayDetailId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Type { get; set; }
        public int PayType { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public int CreatedBy { get; set; }
        public long ClientId { get; set; }
        public double AmountPay { get; set; }
        public string DebtStatusName { get; set; }
        public int DebtStatus { get; set; }
        public List<CountStatus> ListOrders { get; set; }
    }
}
