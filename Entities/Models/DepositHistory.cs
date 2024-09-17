using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class DepositHistory
{
    public int Id { get; set; }

    /// <summary>
    /// Thời gian giao dịch
    /// </summary>
    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateLast { get; set; }

    /// <summary>
    /// User nạp trans. Lấy user login
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Mã giao dịch
    /// </summary>
    public string TransNo { get; set; }

    /// <summary>
    /// Tiêu đề nạp
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Số tiền nạp
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// Loại giao dịch
    /// </summary>
    public short? TransType { get; set; }

    /// <summary>
    /// HÌnh thức thanh toán
    /// </summary>
    public short? PaymentType { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Ảnh ủy nhiệm chi
    /// </summary>
    public string ImageScreen { get; set; }

    public short? ServiceType { get; set; }

    public string BankName { get; set; }

    public long? UserVerifyId { get; set; }

    public DateTime? VerifyDate { get; set; }

    public string NoteReject { get; set; }

    public long? ClientId { get; set; }

    public string BankAccount { get; set; }

    public bool? IsFinishPayment { get; set; }
}
