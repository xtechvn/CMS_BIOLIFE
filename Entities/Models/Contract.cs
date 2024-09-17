using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Contract
{
    public long ContractId { get; set; }

    public string ContractNo { get; set; }

    public DateTime ContractDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public int ClientId { get; set; }

    public int SalerId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateLast { get; set; }

    public byte VerifyStatus { get; set; }

    public int? UserIdCreate { get; set; }

    public int? UserIdUpdate { get; set; }

    /// <summary>
    /// AccountClientID là user sẽ duyệt hđ này
    /// </summary>
    public int? UserIdVerify { get; set; }

    /// <summary>
    /// Ngày duyệt hợp đồng
    /// </summary>
    public DateTime? VerifyDate { get; set; }

    /// <summary>
    /// Tổng số lần được duyệt của hợp đồng. Cộng dồn sau mỗi lần duyệt
    /// </summary>
    public int? TotalVerify { get; set; }

    public int? ContractStatus { get; set; }

    /// <summary>
    /// 1: 7 ngày, 2: 15 ngày
    /// </summary>
    public int? DebtType { get; set; }

    public int? UpdatedBy { get; set; }

    public string ServiceType { get; set; }

    public int? PolicyId { get; set; }

    public string Note { get; set; }

    public int? ClientType { get; set; }

    public int? PermisionType { get; set; }

    public bool? IsDelete { get; set; }
}
