using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class PaymentRequestSearchModel
    {
        public string PaymentCode { get; set; }
        public string OrderNo { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int StatusChoose { get; set; }
        public string ServiceCode { get; set; }
        public int PaymentType { get; set; }
        public int SupplierId { get; set; }
        public int ClientId { get; set; }
        public List<int> PaymentTypeMulti { get; set; }
        public List<int> StatusMulti { get; set; }
        public List<int> TypeMulti { get; set; }
        public List<int> CreateByIds { get; set; }
        public List<int> VerifyByIds { get; set; }
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
        public DateTime? PaymentDateFrom
        {
            get
            {
                return DateUtil.StringToDate(PaymentDateFromStr);
            }
        }
        public string PaymentDateFromStr { get; set; }
        public DateTime? PaymentDateTo
        {
            get
            {
                return DateUtil.StringToDate(PaymentDateToStr);
            }
        }
        public string PaymentDateToStr { get; set; }
        public DateTime? VerifyDateFrom
        {
            get
            {
                return DateUtil.StringToDate(VerifyDateFromStr);
            }
        }
        public string VerifyDateFromStr { get; set; }
        public DateTime? VerifyDateTo
        {
            get
            {
                return DateUtil.StringToDate(VerifyDateToStr);
            }
        }
        public string VerifyDateToStr { get; set; }
        public bool? IsSupplierDebt { get; set; }
        public bool? IsPaymentBefore { get; set; }
    }
}
