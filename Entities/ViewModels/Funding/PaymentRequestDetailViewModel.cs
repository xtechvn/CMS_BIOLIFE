using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class PaymentRequestDetailViewModel
    {
        public int Id { get; set; }
        public int PayId { get; set; }
        public int OrderId { get; set; }
        public int? Type { get; set; }
        public int PayDetailId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPayment { get; set; }
        public double TotalNeedPayment { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal ServiceAmount { get; set; }
        public double ServicePrice { get; set; }
        public decimal OrderAmountPay { get; set; }
        public decimal AmountPay { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string PaymentTypeStr { get; set; }
        public string SalerName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string OperatorName { get; set; }
        public string BankIdName { get; set; }
        public string AccountNumber { get; set; }
        public long RequestId { get; set; }
        public long ContractPayId { get; set; }
        public long SupplierId { get; set; }
        public int? Status { get; set; }
        public long ServiceId { get; set; }
        public string GroupBookingId { get; set; }
        public int ServiceType { get; set; }
        public string OrderNo { get; set; }
        public string UserCreateFullName { get; set; }
        public string DepartmentName { get; set; }
        public PaymentRequestDetail PaymentRequestDetail { get; set; }
        public PaymentRequest PaymentRequest { get; set; }
        public DateTime StartDate { get; set; }
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

    }
}
