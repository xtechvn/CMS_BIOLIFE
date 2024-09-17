using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PaymentRequestDetail
{
    public long Id { get; set; }

    public long RequestId { get; set; }

    public long OrderId { get; set; }

    public long? ServiceId { get; set; }

    public int? Type { get; set; }

    public decimal Amount { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string ServiceCode { get; set; }
}
