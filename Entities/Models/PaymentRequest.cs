using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PaymentRequest
{
    public long Id { get; set; }

    public string PaymentCode { get; set; }

    /// <summary>
    /// 1: Thanh toán dịch vụ , 2: Thanh toán khác
    /// </summary>
    public int Type { get; set; }

    public int PaymentType { get; set; }

    public int? SupplierId { get; set; }

    public decimal Amount { get; set; }

    public long? BankingAccountId { get; set; }

    public string Description { get; set; }

    public string Note { get; set; }

    public bool? IsServiceIncluded { get; set; }

    public int? Status { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? UserVerify { get; set; }

    public DateTime? VerifyDate { get; set; }

    public int? ClientId { get; set; }

    public string DeclineReason { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsSupplierDebt { get; set; }

    public string BankAccount { get; set; }

    public string BankName { get; set; }

    public string AbandonmentReason { get; set; }

    public bool? IsPaymentBefore { get; set; }
}
