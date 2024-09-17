using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels.TransferSms
{
    public class TransferSmsSearchModel : TransactionSMS
    {
        public string FromDateStr { get; set; }
        public int type { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? FromDate
        {
            get
            {
                if (!string.IsNullOrEmpty(FromDateStr))
                {
                    var lstDate = FromDateStr.Split('-');
                    if (lstDate.Length == 0 || lstDate.Length == 1)
                        lstDate = FromDateStr.Split('/');
                    FromDateStr = lstDate[0] + "/" + lstDate[1] + "/" + lstDate[2];
                    var fromDate = DateUtil.StringToDate(FromDateStr);
                    return new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, 00, 00, 00, DateTimeKind.Local);
                }
                return null;
            }
        }
        public string ToDateStr { get; set; }
        public DateTime? ToDate
        {
            get
            {
                if (!string.IsNullOrEmpty(ToDateStr))
                {
                    var lstDate = ToDateStr.Split('-');
                    if (lstDate.Length == 0 || lstDate.Length == 1)
                        lstDate = ToDateStr.Split('/');
                    ToDateStr = lstDate[0] + "/" + lstDate[1] + "/" + lstDate[2];
                    var toDate = DateUtil.StringToDate(ToDateStr);
                    return new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day, 23, 59, 59, DateTimeKind.Local);
                }
                return null;
            }
        }
        public bool StatusSuccess { get; set; }
        public bool StatusFail { get; set; }
        public bool AmountSuccess { get; set; }
        public bool AmountFail { get; set; }
    }
    public class TransferSmsTotalModel
    {
        public Double Amount { get; set; }
        public Double AmountTransaction { get; set; }
        public Double Balance { get; set; }
        public Double Total { get; set; }
        public int Month { get; set; }   
    }
}
