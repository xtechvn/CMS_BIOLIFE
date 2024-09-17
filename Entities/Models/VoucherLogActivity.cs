using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class VoucherLogActivity
{
    public int Id { get; set; }

    public int? VoucherId { get; set; }

    public int? OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Trang thai giao dịch voucher. 1: khoa. 0: dang ap dung
    /// </summary>
    public int? Status { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? StoreId { get; set; }

    public double? PriceSaleVnd { get; set; }

    public int? CartId { get; set; }

    public virtual Voucher Voucher { get; set; }
}
