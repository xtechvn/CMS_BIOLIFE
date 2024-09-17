using Entities.Models;
using System;
using System.Collections.Generic;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class PaymentVoucherViewModel : PaymentVoucher
    {
        public int PaymentRequestId { get; set; }
        public int BankingAccountSource { get; set; }
        public List<int> CreateByIds { get; set; }
        public string Content { get; set; }
        public string PaymentVoucherType { get; set; }
        public string PaymentTypeStr { get; set; }
        public string SupplierName { get; set; }
        public string BankIdName { get; set; }
        public string AccountNameSource { get; set; }
        public string AccountNumberSource { get; set; }
        public string AccountNumber { get; set; }
        public string BankIdSource { get; set; }
        public string CreatedByName { get; set; }
        public string UserUpdateName { get; set; }
        public string ClientName { get; set; }
        public string PaymentRequestCode { get; set; }
        public string UserCreatePaymentRequest { get; set; }
        public decimal AmountPaymentRequest { get; set; }
        public string UserName { get; set; }
        public long TotalRow { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CreatedDateFrom
        {
            get
            {
                if (!string.IsNullOrEmpty(CreatedDateFromStr))
                    return DateUtil.StringToDate(CreatedDateFromStr);
                return null;
            }
        }
        public string CreatedDateFromStr { get; set; }
        public string CreatedDateToStr { get; set; }
        public DateTime? CreatedDateTo
        {
            get
            {
                if (!string.IsNullOrEmpty(CreatedDateToStr))
                    return DateUtil.StringToDate(CreatedDateToStr);
                return null;
            }
        }
        public List<PaymentRequest> RelateRequest { get; set; }
        public List<PaymentRequestViewModel> PaymentRequestDetails { get; set; }
        public List<int> PaymentTypeMulti { get; set; }
        public List<int> TypeMulti { get; set; }
        public List<AttachFile> AttachFile { get; set; }
        public IEnumerable<string> OtherImages { get; set; }

    }
}
