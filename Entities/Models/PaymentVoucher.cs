using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PaymentVoucher
{
    public long Id { get; set; }

    public string PaymentCode { get; set; }

    /// <summary>
    /// 1: Thanh toán dịch vụ , 2: Thanh toán khác
    /// </summary>
    public int Type { get; set; }

    public int PaymentType { get; set; }

    /// <summary>
    /// Id Phiếu yêu cầu chi
    /// </summary>
    public string RequestId { get; set; }

    public long SupplierId { get; set; }

    public decimal Amount { get; set; }

    public long BankingAccountId { get; set; }

    public string Description { get; set; }

    public string Note { get; set; }

    public int? ClientId { get; set; }

    public string AttachFiles { get; set; }

    public string BankAccount { get; set; }

    public string BankName { get; set; }

    public int? SourceAccount { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
