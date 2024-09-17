using System;
using System.Collections.Generic;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class FundingSearch
    {
        public string TransNo { get; set; }
        public int TransType { get; set; }
        public int ServiceType { get; set; }
        public int PaymentType { get; set; }
        public string TransactionDate { get; set; }
        public List<long> CreateByIds { get; set; }
        public List<int> PaymentTypes { get; set; }
        public List<int> ServiceTypes { get; set; }
        public List<int> TransTypes { get; set; }
        public string CreateBy { get; set; }
        public string Approver { get; set; }
        public List<long> ApproverIds { get; set; }
        public string ApproveDate { get; set; }
        public int Status { get; set; }
        public int StatusChoose { get; set; }
        public List<int> StatusList { get; set; }
        public string OrderBy { get; set; }
        public string SortBy { get; set; }
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
        public DateTime? FromApproveDate
        {
            get
            {
                return DateUtil.StringToDate(FromApproveDateStr);
            }
        }
        public string FromApproveDateStr { get; set; }
        public DateTime? ToApproveDate
        {
            get
            {
                return DateUtil.StringToDate(ToApproveDateStr);
            }
        }
        public string ToApproveDateStr { get; set; }

    }
}
