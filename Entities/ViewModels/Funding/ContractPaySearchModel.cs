using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class ContractPaySearchModel
    {
        public string OrderNo { get; set; }
        public string Status { get; set; }
        public string ServiceCode { get; set; }
        public string LabelName { get; set; }
        public string BillNo { get; set; }
        public int? ClientId { get; set; }
        public long SupplierId { get; set; }
        public int EmployeeId { get; set; }
        public string Note { get; set; }
        public double Amount { get; set; }
        public short? Type { get; set; }
        public short? PayType { get; set; }
        public int? BankingAccountId { get; set; }
        public string Description { get; set; }
        public string AttatchmentFile { get; set; }
        public DateTime? ExportDate { get; set; }
        public short PayStatus { get; set; }
        public int? CreatedBy { get; set; }
        public string Content { get; set; }
        public string ClientName { get; set; }
        public string CreateByName { get; set; }
        public List<int> CreateByIds { get; set; }
        public List<int> StatusMulti { get; set; }
        public DateTime? FromCreateDate
        {
            get
            {
                return DateUtil.StringToDate(FromCreateDateStr);
            }
        }
        public string FromCreateDateStr { get; set; }
        public DateTime? ToCreateDate
        {
            get
            {
                return DateUtil.StringToDate(ToCreateDateStr);
            }
        }
        public string ToCreateDateStr { get; set; }
        public int DebtStatus { get; set; }
    }
}
