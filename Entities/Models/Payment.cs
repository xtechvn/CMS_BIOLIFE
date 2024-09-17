using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Payment
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long ClientId { get; set; }

    public double Amount { get; set; }

    public int PaymentType { get; set; }

    public DateTime PaymentDate { get; set; }

    public string Note { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string TransferContent { get; set; }

    /// <summary>
    /// loại thanh toán cho đối tượng nào. Đơn hàng hay nạp quỹ
    /// </summary>
    public short? DepositPaymentType { get; set; }

    public string BotPaymentScreenShot { get; set; }

    public string ImageScreenShot { get; set; }

    public string BankName { get; set; }
}
