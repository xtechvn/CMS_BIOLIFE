using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class OrderBak
{
    public long OrderId { get; set; }

    public string OrderNo { get; set; }

    public byte? ServiceType { get; set; }

    public DateTime? CreateTime { get; set; }

    public double? Amount { get; set; }

    public int? PaymentStatus { get; set; }

    public long? ClientId { get; set; }

    public long? ContactClientId { get; set; }

    public byte? OrderStatus { get; set; }

    public long? ContractId { get; set; }

    public string SmsContent { get; set; }

    public int? PaymentType { get; set; }

    public string BankCode { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string PaymentNo { get; set; }

    public string ColorCode { get; set; }

    public double? Discount { get; set; }

    public double? Profit { get; set; }

    public DateTime? ExpriryDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string ProductService { get; set; }

    public string Note { get; set; }

    public int? UtmSource { get; set; }

    public DateTime? UpdateLast { get; set; }

    public long? SalerId { get; set; }

    public string SalerGroupId { get; set; }

    public long? UserUpdateId { get; set; }

    public short? SystemType { get; set; }

    public long? AccountClientId { get; set; }

    public long? CreatedBy { get; set; }

    public string Description { get; set; }

    public short? BranchCode { get; set; }

    public string BookingInfo { get; set; }

    public string Label { get; set; }

    public short? IsFinishPayment { get; set; }

    public int? PercentDecrease { get; set; }

    public int? VoucherId { get; set; }

    public double? Price { get; set; }

    public int? SupplierId { get; set; }

    public int? DepartmentId { get; set; }
}
