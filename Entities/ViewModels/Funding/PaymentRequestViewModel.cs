using Entities.Models;
using Entities.ViewModels.Funding;
using System;
using System.Collections.Generic;

namespace Entities.ViewModels
{
    public class PaymentRequestViewModel : PaymentRequest
    {
        public string RequestIds { get; set; }
        public int RequestId { get; set; }
        public string PaymentVoucherNo { get; set; }
        public string PaymentVoucherCode { get; set; }
        public long PaymentVoucherId { get; set; }
        public double PaymentVoucherAmount { get; set; }
        public double ServiceAmount { get; set; }
        public double Price { get; set; }
        public int PaymentRequestId { get; set; }
        public string PaymentRequestType { get; set; }
        public string PaymentTypeStr { get; set; }
        public string UserName { get; set; }
        public string ClientName { get; set; }
        public string SupplierName { get; set; }
        public string BankIdName { get; set; }
        public string AccountNumber { get; set; }
        public string UserVerifyName { get; set; }
        public string UserCreateName { get; set; }
        public string UserCreatePaymentRequest { get; set; }
        public string PaymentRequestCode { get; set; }
        public string ListServiceCode { get; set; }
        public string ListServiceId { get; set; }
        public string CongNo { get; set; }
        public string TTKhachTT { get; set; }
        public List<CountStatus> ListServiceCodeAndType { get; set; }
        public string PaymentRequestStatus { get; set; }
        public string PaymentDateStr { get; set; }
        public string PaymentDateRemind
        {
            get
            {
                if (PaymentDate == null) return string.Empty;
                if (PaymentDate < DateTime.Today)
                {
                    var time = DateTime.Today.Subtract(PaymentDate.Value);
                    return "Quá " + time.Days + " ngày";
                }
                if (PaymentDate == DateTime.Today)
                {
                    return "Thanh toán trong hôm nay";
                }
                if (PaymentDate > DateTime.Today)
                {
                    var time = PaymentDate.Value.Subtract(DateTime.Today);
                    return "Còn " + time.Days + " ngày";
                }
                return string.Empty;
            }
        }
        public int IsSend { get; set; }
        public string PaymentDateViewStr
        {
            get
            {
                if (PaymentDate != null)
                    return PaymentDate.Value.ToString("dd/MM/yyyy HH:mm");
                return string.Empty;
            }
        }
        public string DataContent { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public long TotalRow { get; set; }
        public List<PaymentRequestDetailViewModel> PaymentRequestDetails { get; set; }
        public List<PaymentRequestDetailViewModel> RelateData { get; set; }
        public int OrderId { get; set; }
        public long ServiceId { get; set; }
        public int ServiceType { get; set; }
        public string ServiceCode { get; set; }
        public string GroupBookingId { get; set; }
        public DateTime StartDate { get; set; }
        public int IsIncludeService { get; set; }
        public string StartDateStr
        {
            get
            {
                if (StartDate != null)
                    return StartDate.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }
        public DateTime EndDate { get; set; }
        public string EndDateStr
        {
            get
            {
                {
                    if (EndDate != null)
                        return EndDate.ToString("dd/MM/yyyy");
                    return string.Empty;
                }
            }
        }
        public string OrderNo { get; set; }
        public decimal AmountPay { get; set; }
        public decimal AmountReturn { get; set; }
        public decimal AmountPayment { get; set; }
        public decimal Payment { get; set; }
        public decimal AmountPaymentRequest { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal OrderAmountPay { get; set; }
        public string SalerName { get; set; }
        public string OperatorName { get; set; }
        public decimal TotalNeedPayment { get; set; }
        public decimal TotalDisarmed { get; set; }
        public decimal TotalAmount { get; set; }
        public string ServiceName { get; set; }
        public string UserCreateFullName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsEditAmountReject { get; set; }
        public bool IsAdminEdit { get; set; }
        public decimal TotalSupplierRefund { get; set; }
        public decimal TotalAmountService { get; set; }
    }
}
