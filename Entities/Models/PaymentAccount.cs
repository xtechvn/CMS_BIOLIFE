using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PaymentAccount
{
    public int Id { get; set; }

    /// <summary>
    /// Số tài khoản
    /// </summary>
    public string AccountNumb { get; set; }

    /// <summary>
    /// Tên chủ tài khoản
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// Tên ngân hàng
    /// </summary>
    public string BankName { get; set; }

    /// <summary>
    /// Chi nhánh
    /// </summary>
    public string Branch { get; set; }

    public long ClientId { get; set; }

    public int? IsDelete { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
