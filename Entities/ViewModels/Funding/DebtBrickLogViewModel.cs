using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class DebtBrickLogViewModel
    {
        public long ContractPayId { get; set; }
        public string ContractPayBillNo { get; set; }
        public long OrderId { get; set; }
        public string ClientName { get; set; }
        public int? ClientId { get; set; }
        public string OrderNo { get; set; }
        public string Note { get; set; }
        public double AmountOrder { get; set; }
        public double AmountRemain { get; set; }
        public double Amount { get; set; }
        public double Payment { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ObjectType { get; set; }
    }
}
