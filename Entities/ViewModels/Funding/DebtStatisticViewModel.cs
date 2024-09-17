using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class DebtStatisticViewModel : DebtStatistic
    {
        public List<DebtStatisticViewModel> RelateData { get; set; }
        public List<int> CreateByIds { get; set; }
        public List<int> StatusMulti { get; set; }
        public string StatusName { get; set; }
        public string UserCreateName { get; set; }
        public string CreateByName { get; set; }
        public string ClientName { get; set; }
        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }
        public double AmountPay { get; set; }
        public double Payment { get; set; }
        public double NotPayment { get; set; }
        public int IsSend { get; set; }
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public string SalerPermission { get; set; }
        public string UserUpdateName { get; set; }
        public string UserVerifyName { get; set; }
        public bool IsChecked { get; set; }
        public string OperatorIdName { get; set; }
        public string CreateDateStr
        {
            get
            {
                if (CreatedDate != null) return CreatedDate.Value.ToString("dd/MM/yyyy HH:mm");
                return string.Empty;
            }
        }
        public string FromDateStr
        {
            get
            {
                if (FromDate != null) return FromDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    FromDate = DateUtil.StringToDateTime(value, "dd/MM/yyyy");
            }
        }
        public string ToDateStr
        {
            get
            {
                if (ToDate != null) return ToDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    ToDate = DateUtil.StringToDateTime(value, "dd/MM/yyyy");
            }
        }
    }
}
